using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Models.Projects;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class UserRepository
    {
        private readonly RentContext _context;
        private readonly UserContext _userContext;
        private readonly LocationContext _locationContext;
        private readonly LocationUserContext _locationUserContext;
        private readonly PropCondition _propCondition;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly FirestoreCommunicationRepository _firestoreRepository;

        public UserRepository(RentContext rentContext, UserContext userContext, LocationContext locationContext, LocationUserContext locationUserContext, PropCondition propCondition, IRoleAuthenticationRepository roleRepo, ProjectRoleRepository projectRoleRepository, FirestoreCommunicationRepository firestoreRepository)
        {
            _context = rentContext;
            _userContext = userContext;
            _locationContext = locationContext;
            _locationUserContext = locationUserContext;
            _propCondition = propCondition;
            _roleRepo = roleRepo;
            _projectRoleRepository = projectRoleRepository;
            _firestoreRepository = firestoreRepository;
        }

        public IQueryable<dynamic> GetUsersInvitedToWork(int requester, int workId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                return _context.User.Where(u => u.Contracts.Any(c => c.WorkInvitations.Any(wi => wi.WorkID == workId))).Select(User.StandardDTO(null));
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public object Get(int requester, int userId)
        {
            var user = _userContext.DetailedOne(requester, (u) => u.ID == userId, "Role");
            return user;
        }

        public IEnumerable<dynamic> GetAll(int requester)
        {
            var users = _context.User.Select(User.StandardDTO(null));
            return users;
        }

        public IEnumerable<dynamic> GetOfProjectAvailableOnDate(int requester, int projectId, DateTime date)
        {
            var users = _context.ProjectUser.Where(pu => pu.ProjectID == projectId && pu.User.ClientID == null)
                .Select(pu => pu.User)
                .Select(User.StandardDTO(date));
            return users;
        }

        public IEnumerable<dynamic> GetOfProject(int requester, int projectId)
        {
            var users = _context.ProjectUser.Where(pu => pu.ProjectID == projectId)
                .Select(pu => pu.User)
                .Select(User.StandardDTO(null));
            return users;
        }

        public IEnumerable<dynamic> GetOfNotProject(int requester, int projectId)
        {
            return _context.User.Where(u => !u.ProjectUsers.Any(pu => pu.ProjectID == projectId))
                .Select(User.StandardDTO(null));
        }

        public async Task UpdateUser(int requester, int userId, User updatedUser)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var u = _context.User.Find(userId);
                if (u == null) throw new NotFoundException();

                u.FirstName = updatedUser.FirstName;
                u.LastName = updatedUser.LastName;
                u.ImageLocation = updatedUser.ImageLocation;

                if (u.ClientID == null)
                {
                    u.Email = updatedUser.Email;
                    u.Comment = updatedUser.Comment;
                    u.EmployeeNumber = updatedUser.EmployeeNumber;
                    u.Title = updatedUser.Title;
                    u.LanguageCode = updatedUser.LanguageCode;
                }
                _context.User.Update(u);
                await _context.SaveChangesAsync();
            }
            else throw new UnauthorizedAccessException();
        }

        public async Task UpdateUserImage(int requester, int userId, string userImage)
        {
            await _userContext.Update(requester, u => u.ID == userId, u =>
            {
                u.ImageLocation = userImage;
            });
        }

        public async Task Disable(int requester, int userId)
        {
            var user = _context.User.Include(u => u.ProjectRole).FirstOrDefault();
            if (!_roleRepo.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();
            if (user == null) throw new NotFoundException();
            user.Disabled = true;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            if (user.ProjectRole.HasAllPermissions)
            {
                await _firestoreRepository.RemoveAdmin(userId);
            }
        }

        public async Task Enable(int requester, int userId)
        {
            var user = _context.User.Include(u => u.ProjectRole).FirstOrDefault();
            if (!_roleRepo.IsAdminOrManager(requester)) throw new UnauthorizedAccessException();
            if (user == null) throw new NotFoundException();
            user.Disabled = true;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            if (user.ProjectRole.HasAllPermissions)
            {
                await _firestoreRepository.AddAdmin(userId);
            }
        }

        public async Task AddUsersToProject(int requester, int projectId, ICollection<int> userIds)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Dictionary<int, List<FirestoreConversation>> oldDictionary = new Dictionary<int, List<FirestoreConversation>>();

                for (int i = 0; i < userIds.Count; i++)
                {
                    var userId = userIds.ToList()[i];
                    var cons = _context.User.Where(u => u.ID == userId)
                        .SelectMany(u => u.ProjectItemUsers
                            .Where(piu => piu.ProjectItem.ProjectItemType == ProjectItemType.FirestoreConversations).Select(piu => piu.ProjectItem.FirestoreConversations)).ToList();
                    oldDictionary.Add(userId, cons);
                }

                var projectUsers = userIds.Select(uId => new ProjectUser
                {
                    ProjectID = projectId,
                    UserID = uId,
                });
                _context.ProjectUser.AddRange(projectUsers);
                await _context.SaveChangesAsync();

                var users = _context.User.Where(u => u.ProjectItemUsers
                    .Any(piu => piu.ProjectItem.ProjectID == projectId && piu.ProjectItem.ProjectItemType == ProjectItemType.FirestoreConversations));
                _projectRoleRepository.UpdateProjectItemUsersFromProjectID(projectId);

                for (int i = 0; i < userIds.Count; i++)
                {
                    var userId = userIds.ToList()[i];
                    var user = _context.User.Find(userId);
                    var oldConversations = oldDictionary[userId];
                    var newConversations = _context.User.Where(u => u.ID == userId)
                        .SelectMany(u => u.ProjectItemUsers
                            .Where(piu => piu.ProjectItem.ProjectItemType == ProjectItemType.FirestoreConversations).Select(piu => piu.ProjectItem.FirestoreConversations)).ToList();
                    await _firestoreRepository.UpdateUserConversations(user, oldConversations, newConversations);
                }
            }
            else
                throw new UnauthorizedAccessException();
        }

        public async Task RemoveUsersFromProject(int requester, int projectId, ICollection<int> userIds)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Dictionary<int, List<FirestoreConversation>> oldDictionary = new Dictionary<int, List<FirestoreConversation>>();

                for (int i = 0; i < userIds.Count; i++)
                {
                    var userId = userIds.ToList()[i];
                    var cons = _context.User.Where(u => u.ID == userId)
                        .SelectMany(u => u.ProjectItemUsers
                            .Where(piu => piu.ProjectItem.ProjectItemType == ProjectItemType.FirestoreConversations).Select(piu => piu.ProjectItem.FirestoreConversations)).ToList();
                    oldDictionary.Add(userId, cons);
                }

                var projectUser = _context.ProjectUser
                    .Where(pu => pu.ProjectID == projectId)
                    .Where(pu => userIds.Contains(pu.UserID));
                _context.ProjectUser.RemoveRange(projectUser);
                await _context.SaveChangesAsync();

                _projectRoleRepository.UpdateProjectItemUsersFromProjectID(projectId);

                for (int i = 0; i < userIds.Count; i++)
                {
                    var userId = userIds.ToList()[i];
                    var user = _context.User.Find(userId);
                    var oldConversations = oldDictionary[userId];
                    var newConversations = _context.User.Where(u => u.ID == userId)
                        .SelectMany(u => u.ProjectItemUsers
                            .Where(piu => piu.ProjectItem.ProjectItemType == ProjectItemType.FirestoreConversations).Select(piu => piu.ProjectItem.FirestoreConversations)).ToList();
                    await _firestoreRepository.UpdateUserConversations(user, oldConversations, newConversations);
                }
            }
            else
                throw new UnauthorizedAccessException();
        }



        // OLD TO REMOVE

        public IEnumerable<dynamic> GetWithRole(int requester, int roleId)
        {
            return _userContext.Basic(requester, (u) => u.RoleID == roleId, "Role");
        }

        public IEnumerable<dynamic> GetLocationUsers(int requester, int locationId)
        {
            return _locationUserContext.Get(requester, (lu) => lu.LocationID == locationId, "User.Role").Select(lu => lu.ToUser()).ToList().OrderBy(u => u.RoleID);
        }

        public IEnumerable<dynamic> GetCustomerUsers(int requester, int customerId)
        {
            return _userContext.Basic(requester, (u) => u.CustomerID == customerId, "Role");
        }

        public IEnumerable<dynamic> GetPotentialUsersForLocation(int requester, int locationId)
        {
            var location = _locationContext
                .DatabaseOne(requester, (l) => l.ID == locationId);
            var locationUsers = _locationUserContext
                .Get(requester, (lu) => lu.LocationID == locationId).Select(lu => lu.User.ID);

            return _userContext.Basic(requester, (u) => !locationUsers.Contains(u.ID) && ((u.CustomerID == location.CustomerID) || (u.CustomerID == null)), "Role");
        }
    }
}
