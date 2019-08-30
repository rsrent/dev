using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Google.Cloud.Firestore;
using Rent.Data;
using Rent.Models;

using System.Linq;
using Rent.Repositories.TimePlanning;
using Rent.ContextPoint.Exceptions;
using Microsoft.EntityFrameworkCore;
using Rent.Models.TimePlanning;
using Rent.Models.Projects;

namespace Rent.Repositories
{
    public class FirestoreCommunicationRepository
    {
        FirestoreDb _db;
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleAuthenticationRepository;
        private readonly FirebaseNotificationRepository _firebaseNotificationRepository;
        private readonly ProjectRoleRepository _projectRoleRepository;
        public FirestoreCommunicationRepository(RentContext context, IRoleAuthenticationRepository roleAuthenticationRepository, FirebaseNotificationRepository firebaseNotificationRepository, ProjectRoleRepository projectRoleRepository)
        {
            _context = context;
            _roleAuthenticationRepository = roleAuthenticationRepository;
            _firebaseNotificationRepository = firebaseNotificationRepository;
            _db = FirestoreDb.Create("bms-firestore");
            _projectRoleRepository = projectRoleRepository;
        }

        CollectionReference AdminRef() => _db.Collection("organizations").Document("0").Collection("admins");
        CollectionReference ConversationRef() => _db.Collection("organizations").Document("0").Collection("conversations");
        CollectionReference MessageRef(string conversationId) => ConversationRef().Document(conversationId).Collection("messages");

        public IQueryable<dynamic> GetFirestoreConversations(int requester) => _projectRoleRepository.GetConversationsOfUser(requester).Select(FirestoreConversation.StandardDTO());

        public async Task<string> CreateConversationWithUserIds(int requester, ICollection<int> userIds)
        {
            if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            HashSet<int> _userIds = userIds.ToHashSet();
            _userIds.Add(requester);

            var query = ConversationRef().WhereEqualTo("userCount", _userIds.Count).WhereEqualTo("locked", false);

            foreach (int id in _userIds)
            {
                query = query.WhereEqualTo("users." + id + ".active", true);
            }

            var snap = await query.GetSnapshotAsync();

            if (snap.Documents.Count > 0)
            {
                return snap.Documents.FirstOrDefault().Id;
            }

            var users = _context.User.Where(u => _userIds.Contains(u.ID)).ToList();


            if (users.Any(u => u == null)) throw new NotFoundException();


            return await CreateConversation(requester, false, null, users);
        }

        public async Task<string> CreateConversation(int requester, bool locked, string name, ICollection<User> users)
        {
            //if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                { "name", name },
                { "userCount", users.Count },
                { "latestUpdateTime", Timestamp.FromDateTime(DateTime.UtcNow) },
                { "locked", locked },
            };

            Dictionary<string, Dictionary<string, object>> userDictionary = new Dictionary<string, Dictionary<string, object>>();

            foreach (var user in users)
            {
                Dictionary<string, object> _userDictionary = new Dictionary<string, object> {
                        { "active", true },
                        { "latestMessageSeenId", null },
                        { "firstName", user.FirstName},
                        { "lastName", user.LastName },
                        { "employeeNumber", user.EmployeeNumber },
                        { "userRole", user.UserRole },
                    };

                userDictionary.Add(user.ID.ToString(), _userDictionary);
            }
            conversationDictionary.Add("users", userDictionary);
            var newConversation = await ConversationRef().AddAsync(conversationDictionary);

            return newConversation.Id;
        }

        public async Task<dynamic> PostMessage(int requester, string conversationId, FirestoreMessage message)
        {
            var sender = _context.User.Find(requester);
            var conversation = await ConversationRef().Document(conversationId).GetSnapshotAsync();

            Dictionary<string, object> messageDictionary = new Dictionary<string, object> {
                    { "text", message.Text },
                    { "url", message.URL },
                    { "senderId", requester },
                    { "senderTime", Timestamp.FromDateTime(DateTime.UtcNow) },
                };

            await MessageRef(conversationId).AddAsync(messageDictionary);

            Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                    { "latestUpdateTime", Timestamp.FromDateTime(DateTime.UtcNow) },
                    { "latestMessage", messageDictionary },
                };

            await ConversationRef().Document(conversationId).UpdateAsync(conversationDictionary);

            var body = message.URL != null ? "has sent an image" : message.Text;

            var dic = conversation.ToDictionary();

            var name = dic.ContainsKey("name") && dic["name"] != null && dic["name"].ToString().Length > 0 ? "Ny besked i " + dic.GetValueOrDefault("name") : "Ny besked";

            var users = ((Dictionary<string, object>)dic["users"]).Keys.Select(k => "user_" + k).ToList();

            await _firebaseNotificationRepository.PushNotificationTo(users, name, sender.GetName() + ": " + body);

            return true;
        }

        /*
        public async Task<bool> AddUserToConversation(int requester, string conversationId, User user)
        {

            if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            try
            {
                var conversationSnap = await ConversationRef().Document(conversationId).GetSnapshotAsync();
                var conversation = conversationSnap.ToDictionary();
                var userCount = ((long)conversation["userCount"]) + 1;

                Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                    { "userCount", userCount },
                    { "latestUpdateTime", Timestamp.FromDateTime(DateTime.UtcNow) },
                };


                Dictionary<string, object> userDictionary = new Dictionary<string, object>
                {
                    {
                        user.ID.ToString(),
                        new Dictionary<string, object> {
                            { "active", true },
                            { "latestMessageSeenId", null },
                            { "firstName", user.FirstName},
                            { "lastName", user.LastName },
                            { "employeeNumber", user.EmployeeNumber },
                            { "userRole", user.UserRole },
                        }
                    }
                };

                conversationDictionary.Add("users", userDictionary);
                await ConversationRef().Document(conversationId).SetAsync(conversationDictionary, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUserFromConversation(int requester, string conversationId, int userId)
        {
            if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            try
            {
                var conversationSnap = await ConversationRef().Document(conversationId).GetSnapshotAsync();
                var conversation = conversationSnap.ToDictionary();

                var userCount = ((long)conversation["userCount"]) - 1;


                Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                    { "userCount", userCount },
                };

                Dictionary<string, object> userDictionary = new Dictionary<string, object>
                {
                    { userId.ToString(), FieldValue.Delete }
                };

                conversationDictionary.Add("users", userDictionary);
                await ConversationRef().Document(conversationId).SetAsync(conversationDictionary, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        










        public async Task<bool> AddUsersToConversation(int requester, string conversationId, List<User> users)
        {

            if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            try
            {
                var conversationSnap = await ConversationRef().Document(conversationId).GetSnapshotAsync();
                var conversation = conversationSnap.ToDictionary();
                var userCount = ((long)conversation["userCount"]) + users.Count;

                Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                    { "userCount", userCount },
                    { "latestUpdateTime", Timestamp.FromDateTime(DateTime.UtcNow) },
                };

                Dictionary<string, object> usersDictionary = new Dictionary<string, object>();

                users.ForEach(user =>
                {



                    usersDictionary.Add(user.ID.ToString(), new Dictionary<string, object> {
                            { "active", true },
                            { "latestMessageSeenId", null },
                            { "firstName", user.FirstName},
                            { "lastName", user.LastName },
                            { "employeeNumber", user.EmployeeNumber },
                            { "userRole", user.UserRole },
                        });
                });

                conversationDictionary.Add("users", usersDictionary);
                await ConversationRef().Document(conversationId).SetAsync(conversationDictionary, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }

        public async Task<bool> RemoveUsersFromConversation(int requester, string conversationId, List<int> userIds)
        {
            if (!_roleAuthenticationRepository.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();

            try
            {
                var conversationSnap = await ConversationRef().Document(conversationId).GetSnapshotAsync();
                var conversation = conversationSnap.ToDictionary();

                var userCount = ((long)conversation["userCount"]) - 1;


                Dictionary<string, object> conversationDictionary = new Dictionary<string, object> {
                    { "userCount", userCount },
                };

                Dictionary<string, object> usersDictionary = new Dictionary<string, object>();

                userIds.ForEach(userId =>
                {
                    usersDictionary.Add(userId.ToString(), FieldValue.Delete);
                });

                conversationDictionary.Add("users", usersDictionary);
                await ConversationRef().Document(conversationId).SetAsync(conversationDictionary, SetOptions.MergeAll);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error:");
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
        */




        Dictionary<string, object> AddUsersDictionary(params User[] users)
        {
            Dictionary<string, object> usersDictionary = new Dictionary<string, object>();

            users.ToList().ForEach(user =>
            {
                usersDictionary.Add(user.ID.ToString(), new Dictionary<string, object> {
                        { "active", true },
                        { "latestMessageSeenId", null },
                        { "firstName", user.FirstName},
                        { "lastName", user.LastName },
                        { "employeeNumber", user.EmployeeNumber },
                        { "userRole", user.UserRole },
                    });
            });

            return new Dictionary<string, object>
            {
                { "userCount", FieldValue.Increment(users.Length) },
                { "users", usersDictionary },
            };
        }

        Dictionary<string, object> RemoveUsersDictionary(params User[] users)
        {
            Dictionary<string, object> usersDictionary = new Dictionary<string, object>();

            users.ToList().ForEach(userId =>
            {
                usersDictionary.Add(userId.ToString(), FieldValue.Delete);
            });

            return new Dictionary<string, object>
            {
                { "userCount", FieldValue.Increment(- users.Length) },
                { "users", usersDictionary },
            };
        }

        public async Task<bool> UpdateUserConversations(User user, List<FirestoreConversation> oldConversations, List<FirestoreConversation> newConversations)
        {
            WriteBatch batch = _db.StartBatch();

            oldConversations.Where(o => !newConversations.Any(n => o.ID == n.ID)).ToList()
                .ForEach(c =>
                {
                    batch.Set(
                        ConversationRef().Document(c.ConversationID), RemoveUsersDictionary(user), SetOptions.MergeAll);

                });

            newConversations.Where(o => !oldConversations.Any(n => o.ID == n.ID)).ToList()
                .ForEach(c =>
                {
                    batch.Set(
                        ConversationRef().Document(c.ConversationID), AddUsersDictionary(user), SetOptions.MergeAll);

                });

            await batch.CommitAsync();
            return true;
        }

        public async Task<bool> UpdateConversationUsers(FirestoreConversation conversation, List<User> oldUsers, List<User> newUsers)
        {
            await ConversationRef().Document(conversation.ConversationID).SetAsync(RemoveUsersDictionary(oldUsers.Where(o => !newUsers.Any(n => o.ID == n.ID)).ToArray()), SetOptions.MergeAll);
            await ConversationRef().Document(conversation.ConversationID).SetAsync(AddUsersDictionary(newUsers.Where(o => !oldUsers.Any(n => o.ID == n.ID)).ToArray()), SetOptions.MergeAll);
            return true;
        }

        public async Task AddAdmin(int userID)
        {
            await AdminRef().Document(userID.ToString()).CreateAsync(new Dictionary<string, object>
            {
                { "id", userID.ToString() }
            });
        }

        public async Task RemoveAdmin(int userID)
        {
            await AdminRef().Document(userID.ToString()).DeleteAsync();
        }
    }
}
