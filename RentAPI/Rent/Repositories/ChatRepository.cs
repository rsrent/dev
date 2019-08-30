using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;

namespace Rent.Repositories
{
    public class ChatRepository
    {
        RentContext _context;
        public ChatRepository(RentContext context) {
            _context = context;
        }

        public List<ConversationDTO> GetUserConversations(int userID)
        {
            var user = _context.User.Find(userID);
            if (user == null || user.Disabled)
            {
                return null;
            }

            var conversations = 
                _context.ConversationUsers
                        .Include(cu => cu.Conversation).ThenInclude(con => con.NewestMessage)
                        .Where(cu => cu.UserID == userID)
                        .Select(cu => 
                        new ConversationDTO 
                        {
                            ID = cu.ConversationID, 
                            Title = cu.Conversation.Title, 
                            NewestMessage = cu.Conversation.NewestMessage,
                            LastSeenMessageID = cu.LastSeenMessageID,
                            Open = cu.Conversation.Open,
                            Users = _context.ConversationUsers
                                            .Include(u => u.User).ThenInclude(u => u.Role)
                                            .Where(cu2 => cu2.ConversationID == cu.ConversationID)
                                            .Select(u => u.User).ToList()
                        });

            return conversations.ToList();
        }

        public async Task<List<ConversationDTO>> GetLocationConversations(int userID, int locationID)
        {
            var location = await _context.Location.FindAsync(locationID);
            if (location == null || location.Disabled)
                return null;

            var user = _context.User.Find(userID);
            if (user == null || user.Disabled)
                return null;

            var conversations =
                _context.ConversationUsers
                        .Include(cu => cu.Conversation).ThenInclude(con => con.NewestMessage)
                        .Where(cu => cu.UserID == userID && (cu.ConversationID == location.TeamConversationID || cu.ConversationID == location.CustomerConversationID))
                        .Select(cu =>
                        new ConversationDTO
                        {
                            ID = cu.ConversationID,
                            Title = cu.Conversation.Title,
                            NewestMessage = cu.Conversation.NewestMessage,
                            LastSeenMessageID = cu.LastSeenMessageID,
                            Open = cu.Conversation.Open,
                            Users = _context.ConversationUsers
                                            .Include(u => u.User).ThenInclude(u => u.Role)
                                            .Where(cu2 => cu2.ConversationID == cu.ConversationID)
                                            .Select(u => u.User).ToList()
                        });

            return conversations.ToList();
        }

        public List<ConversationUsers> GetConversationUsers(int conversationID) {
            var users =
                _context.ConversationUsers
                        .Include(cu => cu.User)
                        .Where(cu => cu.ConversationID == conversationID);
            return users.ToList();
        }
        
        public bool AddUserToConversation(int? userID, int? conversationID)
        {
            if (conversationID == null || userID == null)
                return false;
            return AddUserToConversation((int)userID, (int)conversationID);
        }

        public bool AddUserToConversation(int? userID, int conversationID)
        {
            if (userID == null)
                return false;
            return AddUserToConversation((int)userID, conversationID);
        }

        public bool AddUserToConversation(int userID, int? conversationID)
        {
            if (conversationID == null)
                return false;
            return AddUserToConversation(userID, (int)conversationID);
        }

        public bool AddUserToConversation(int userID, int conversationID) {
            var user = _context.User.Find(userID);
            var conversation = _context.Conversation.Find(conversationID);
            if (user == null || conversation == null)
                return false;

            var oldConversationUser = _context.ConversationUsers.Find(conversationID, userID);
            if(oldConversationUser != null) {
                return true;
            }

            var conversationUser = new ConversationUsers { ConversationID = conversationID, UserID = userID, NotificationsOn = true };
            _context.ConversationUsers.Add(conversationUser);
            _context.SaveChanges();
            return true;
        }

        public bool RemoveUserFromConversation(int? userID, int? conversationID)
        {
            if (conversationID == null || userID == null)
                return false;
            return RemoveUserFromConversation((int)userID, (int)conversationID);
        }

        public bool RemoveUserFromConversation(int? userID, int conversationID)
        {
            if (userID == null)
                return false;
            return RemoveUserFromConversation((int)userID, conversationID);
        }

        public bool RemoveUserFromConversation(int userID, int? conversationID)
        {
            if (conversationID == null)
                return false;
            return RemoveUserFromConversation(userID, (int)conversationID);
        }

        public bool RemoveUserFromConversation(int userID, int conversationID)
        {
            var conversationUser = _context.ConversationUsers.Find(conversationID, userID);
            if (conversationUser == null)
                return true;
            _context.ConversationUsers.Remove(conversationUser);
            _context.SaveChanges();
            return true;
        }
    }
}
