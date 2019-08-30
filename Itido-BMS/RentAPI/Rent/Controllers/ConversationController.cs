using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using System.Data.Entity;
using Rent.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace Rent.Controllers
{
    [Produces("application/json")]
    
    [Route("api/Conversation")]
    public class ConversationController : Controller
    {
        private string ThisPermission = "Chat";
        private readonly RentContext _context;
        private readonly ChatRepository _chatRepository;
        private readonly PermissionRepository _permissionRepository;

        public ConversationController(RentContext context, ChatRepository chatRepository, PermissionRepository permissionRepository)
        {
            _context = context;
            _chatRepository = chatRepository;
            _permissionRepository = permissionRepository;
        }

        // GET: api/Conversation/1/Limit/10/InitialMessage/1
        [HttpGet("{conversationID}/{limit}/{initialMessageId}")]
        [Authorize]
        public async Task<IActionResult> GetChatMessages([FromRoute] int conversationID, [FromRoute] int limit, [FromRoute] int initialMessageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            //var userID = Int32.Parse(User.Claims.ToList()[0].Value);
            if (initialMessageId == 0)
            {
                initialMessageId = int.MaxValue;
            }

            var messagesTemp = _context.Message
                                       .Where(m => m.ConversationID == conversationID && m.ID < initialMessageId)
                                   .OrderByDescending(m => m.ID)
                                   .Take(limit);

            foreach (var m in messagesTemp)
            {
                if (m.SpecialMessageID != null)
                    m.SpecialMessage = _context.SpecialMessage.FirstOrDefault(sm => sm.ID == m.SpecialMessageID);
            }

            var messages = messagesTemp.Select(m => MessageDTO.MessageDTOFactory(m)).Where(m => m != null);

            if (messages != null && messages.Count() > 0)
                await MessageSeen(messages.Max(m => m.ID));
            /*
            var conversationUser = _context.ConversationUsers.Find(conversationID, userID);

            if(conversationUser != null && messages.Count() > 0)
            {
                var newestMessageID = messages.Max(m => m.ID);
                if(conversationUser.LastSeenMessageID == null || conversationUser.LastSeenMessageID < newestMessageID) {
                    conversationUser.LastSeenMessageID = newestMessageID;
                    conversationUser.UnseenMessages = 0;
                    _context.ConversationUsers.Update(conversationUser);
                    _context.SaveChanges();
                }
            }

            if (messages == null)
            {
                return NotFound();
            }*/

            //System.Diagnostics.Debug.WriteLine("messages: " + messages.Count() + ", from: " + messages.First().ID + " to: " + messages.Last().ID);

            return Ok(messages);
        }

        [HttpGet("MessagesNewerThan/{conversationID}/{messageId}")]
        [Authorize]
        public async Task<IActionResult> MessagesNewerThan([FromRoute] int conversationID, [FromRoute] int messageId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            //var userID = Int32.Parse(User.Claims.ToList()[0].Value);

            var messagesTemp = _context.Message
                                       .Where(m => m.ConversationID == conversationID && m.ID > messageId)
                                       .OrderByDescending(m => m.ID);

            foreach (var m in messagesTemp)
            {
                if (m.SpecialMessageID != null)
                    m.SpecialMessage = _context.SpecialMessage.FirstOrDefault(sm => sm.ID == m.SpecialMessageID);
            }

            var messages = messagesTemp.Select(m => MessageDTO.MessageDTOFactory(m)).Where(m => m != null);

            if(messages != null && messages.Count() > 0)
                await MessageSeen(messages.Max(m => m.ID));
            /*
            var conversationUser = _context.ConversationUsers.Find(conversationID, userID);

            if (conversationUser != null && messages.Count() > 0)
            {
                var newestMessageID = messages.Max(m => m.ID);
                if (conversationUser.LastSeenMessageID == null || conversationUser.LastSeenMessageID < newestMessageID)
                {
                    conversationUser.LastSeenMessageID = newestMessageID;
                    conversationUser.UnseenMessages = 0;
                    _context.ConversationUsers.Update(conversationUser);
                    _context.SaveChanges();
                }
            }

            if (messages == null)
            {
                return NotFound();
            }*/

            //System.Diagnostics.Debug.WriteLine("messages: " + messages.Count() + ", from: " + messages.First().ID + " to: " + messages.Last().ID);

            return Ok(messages);
        }

        [HttpPut("MessageSeen/{messageID}")]
        [Authorize]
        public async Task<IActionResult> SetMessageSeen([FromRoute] int messageID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var success = await MessageSeen(messageID);

            if(!success) {
                return NotFound();
            }

            return Ok();
        }

        async Task<bool> MessageSeen(int messageID)
        {

            var userID = Int32.Parse(User.Claims.ToList()[0].Value);

            var message = _context.Message.Find(messageID);
            if (message == null)
            {
                return false;
            }

            var conversationUser = _context.ConversationUsers.FirstOrDefault(cu => cu.ConversationID == message.ConversationID && cu.UserID == userID);

            if (conversationUser == null)
            {
                return false;
            }

            conversationUser.LastSeenMessageID = messageID;
            _context.ConversationUsers.Update(conversationUser);
            await _context.SaveChangesAsync();
            return true;
        }

        [HttpPut("NotificationsOn/{conversationID}/{on}")]
        [Authorize]
        public async Task<IActionResult> SetNotifications([FromRoute] int conversationID, [FromRoute] bool on)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var userID = Int32.Parse(User.Claims.ToList()[0].Value);

            var conversationUser = _context.ConversationUsers.FirstOrDefault(cu => cu.ConversationID == conversationID && cu.UserID == userID);

            if (conversationUser == null)
            {
                return NotFound("ConversationUser not found, u:" + userID + " - c:" + conversationID);
            }

            conversationUser.NotificationsOn = on;
            _context.ConversationUsers.Update(conversationUser);
            await _context.SaveChangesAsync();
            return Ok();
        }



        // GET: api/Conversation/Messages/1
        [HttpGet("Messages/{id}")]
        [Authorize]
        public async Task<IActionResult> GetChatMessage([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var message = await _context.Message.FindAsync(id);

            if (message == null)
            {
                return NotFound();
            }

            return Ok(message);
        }
        /*
        // GET: api/Conversation/User/1
        [HttpGet("User/{userID}")]
        public IActionResult GetConversationsFromUser([FromRoute] int userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _context.User.Find(userID);
            if(user == null) {
                return NotFound("User not found");
            }

            var conversations = new List<ConversationDTO>();

            try
            {
                if(user.Title == "Master")
                {
                    return Ok(_context.Conversation);
                }

                var customerLocations = _context.Location.Include(l => l.Customer).Where(l => l.ServiceLeaderID == userID || l.CustomerContactID == userID || l.Customer.MainUserID == userID);
                if (customerLocations.Count() > 0)
                {
                    var customerLocationConversations =
                    _context.Conversation
                    .Include(c => c.CustomerLocation)
                    .Where(c => customerLocations.Any(cl => cl.ID == c.CustomerLocationID))
                            .Select(c => new ConversationDTO { ID = c.ID, Title = c.CustomerLocation.Name, Users = new List<User> () });
                    conversations.AddRange(customerLocationConversations);
                }

                var customers = _context.Customer.Where(c => c.MainUserID == userID);
                if (customers.Count() > 0)
                {
                    var customerConversations =
                    _context.Conversation
                    .Include(c => c.Customer)
                    .Where(c => customers.Any(cl => cl.ID == c.CustomerID))
                    .Select(c => new ConversationDTO { ID = c.ID, Title = c.Customer.Name, Users = new List<User>() });
                    conversations.AddRange(customerConversations);
                }

                var teamLocations1 = _context.Location.Include(l => l.ServiceLeader).Where(l => _context.LocationUser.Any(lu => lu.UserID == user.ID && l.ID == lu.LocationID) || l.ServiceLeader.ID == user.ID);
                if(teamLocations1.Count() > 0)
                {
                    var teamLocationConversations1 =
                    _context.Conversation
                    .Include(c => c.TeamLocation)
                    .Where(c => teamLocations1.Any(cl => cl.ID == c.TeamLocationID))
                    .Select(c => new ConversationDTO { ID = c.ID, Title = c.TeamLocation.Name + " team chat", Users = new List<User>() });
                    conversations.AddRange(teamLocationConversations1);
                }

                if (conversations == null)
                {
                    return NotFound();
                }
            } catch (Exception exc)
            {
                BadRequest(exc.Message);
            }

            return Ok(conversations);
        }*/

        // GET: api/Conversation/User/1
        [HttpGet("User/{userID}")]
        [Authorize]
        public IActionResult GetUserConversations([FromRoute] int userID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var conversations = _chatRepository.GetUserConversations(userID);

            if (conversations == null)
            {
                return NotFound();
            }
            return Ok(conversations);
        }

        [HttpGet("Location/{locationID}")]
        [Authorize]
        public async Task<IActionResult> GetLocationConversations([FromRoute] int locationID)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var conversations = await _chatRepository.GetLocationConversations(UserID, locationID);

            if (conversations == null)
            {
                return NotFound();
            }
            return Ok(conversations);
        }

        [HttpGet("ConversationUsers/{conversationId}")]
        [Authorize]
        public IActionResult GetConversationUsers([FromRoute] int conversationId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var users = _chatRepository.GetConversationUsers(conversationId);

            if(users == null) {
                return NotFound();
            }

            return Ok(users);
        }

        // PUT: api/Conversation/1/UserId/5
        [HttpPut("{conversationID}/{userId}")]
        [Authorize]
        public async Task<IActionResult> AddUserToConversation([FromRoute] int conversationID, [FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update))
            {
                return Unauthorized();
            }

            var conversation = _context.Conversation.Find(conversationID);
            if(conversation == null || (conversation != null && !conversation.Open)) {
                return BadRequest("Conversation not found or closed");
            }

            var oldConversationUser = _context.ConversationUsers.Find(conversationID, userId);
            if(oldConversationUser != null) {
                return BadRequest("User already in conversation");
            }

            var conversationUser = new ConversationUsers
            {
                ConversationID = conversationID,
                UserID = userId
            };

            await _context.ConversationUsers.AddAsync(conversationUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        ///api/Conversation/RemoveUser/" + conversationID + "/" + userID
        [HttpPut("RemoveUser/{conversationID}/{userId}")]
        [Authorize]
        public async Task<IActionResult> RemoveUserFromConversation([FromRoute] int conversationID, [FromRoute] int userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var requesterID = Int32.Parse(User.Claims.ToList()[0].Value);
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update) && userId != requesterID)
            {
                return Unauthorized();
            }

            var conversation = _context.Conversation.Find(conversationID);
            if (conversation == null || (conversation != null && !conversation.Open))
            {
                return BadRequest("Conversation not found or closed");
            }

            var oldConversationUser = _context.ConversationUsers.Find(conversationID, userId);
            if (oldConversationUser == null)
            {
                return BadRequest("User not in conversation");
            }

            _context.ConversationUsers.Remove(oldConversationUser);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("UpdateTitle/{conversationID}/{title}")]
        [Authorize]
        public async Task<IActionResult> UpdateTitle([FromRoute] int conversationID, [FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update))
            {
                return Unauthorized();
            }

            var conversation = _context.Conversation.Find(conversationID);
            if (conversation == null || (conversation != null && !conversation.Open))
            {
                return BadRequest("Conversation not found or closed");
            }

            if(!conversation.Open)
            {
                return BadRequest("Can't update this conversations title");
            }

            conversation.Title = title;

            _context.Conversation.Update(conversation);
            await _context.SaveChangesAsync();

            return Ok();
        }

        // POST: /api/Conversation/1
        [HttpPost("{id}")]
        [Authorize]
        public async Task<IActionResult> AddChatMessage([FromRoute] int id, [FromBody] MessageTextDTO messageDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var conversation = await _context.Conversation.FindAsync(id);
            var users = await _context.ConversationUsers
                .Where(c => c.ConversationID == id)
                .Select(u => u.User)
                .ToListAsync();

            if(conversation == null)
            {
                return NotFound();
            }
            var message = new Message
            {
                UserID = messageDTO.UserID,
                ConversationID = messageDTO.ConversationID,
                MessageText = messageDTO.MessageText,
            };
            await _context.Message.AddAsync(message);
            await _context.SaveChangesAsync();

            //await NotificationRepository.SendToUsers(users, messageDTO.MessageText);
            return Ok(); //TODO: Implement createdataction
            //return CreatedAtAction("GetMessage", new { id = message.ID }, message);
        }

        // POST: /api/Conversation
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddConversation([FromBody] NewConversationDTO conversationDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Create))
            {
                return Unauthorized();
            }


            var conversation = new Conversation
            {
                Title = conversationDTO.Title,
                Open = true
            };

            await _context.Conversation.AddAsync(conversation);
            await _context.SaveChangesAsync();

            _context.ConversationUsers.AddRange(conversationDTO.UserIDs.Select(u => new ConversationUsers { ConversationID = conversation.ID, UserID = u, NotificationsOn = true }));
            await _context.SaveChangesAsync();

            return Ok(conversation);//TODO: Should return createdAtAction
        }

        // PUT: api/Conversation/ChangeMessageStatus/1/awaiting
        [HttpPut("ChangeMessageStatus/{id}/{status}")]
        [Authorize]
        public async Task<IActionResult> ChangeMessageStatus([FromRoute] int id, [FromRoute] string status)
        {
            var message = _context.Message.Find(id);

            if (message == null) return NotFound("Message not found");
            if (message.SpecialMessageID == null) return BadRequest("Message is not a special message");
            var specialMessage = _context.SpecialMessage.Find(message.SpecialMessageID);
            var users = _context.ConversationUsers
                .Where(c => c.ConversationID == message.ConversationID)
                .Select(u => u.User);
            
            Status messageStatus = Status.Awaiting;
            if (message.Type.ToLowerInvariant().Equals("meeting"))
            {
                var meeting = specialMessage as MessageMeeting;
                if(Enum.TryParse(status, out messageStatus))
                    meeting.Status = messageStatus;
                else return BadRequest("status not valid");
            }
            else if (message.Type.ToLowerInvariant().Equals("complaint"))
            {
                var meeting = specialMessage as MessageComplaint;
                if (Enum.TryParse(status, out messageStatus))
                    meeting.Status = messageStatus;
                else return BadRequest("status not valid");
            }
            else if(message.Type.ToLowerInvariant().Equals("morework"))
            {
                var meeting = specialMessage as MessageMoreWork;
                if (Enum.TryParse(status, out messageStatus))
                    meeting.Status = messageStatus;
                else return BadRequest("status not valid");
            }
            else
            {
                return BadRequest("Message " + message.Type + " has no status");
            }
            _context.SpecialMessage.Update(specialMessage);
            await _context.SaveChangesAsync();
            string notificationMessage;
            if(messageStatus == Status.Confirmed)
            {
                notificationMessage = message.Type + " was accepted";
            }
            else
            {
                notificationMessage = message.Type + " was denied";
            }
            //await NotificationRepository.SendToUsers(users.ToList(), notificationMessage);

            return Ok("Message status updated to " + status);
        }

        int UserID => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}