using System;
using System.Collections.Generic;

namespace RentAppProject
{
    public class Conversation
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public bool Open { get; set; }
        public ConversationType Type { get; set; }
        public string IdOfType { get; set; }
        public List<Message> Messages { get; set; }
        public List<User> Users { get; set; }
        public Dictionary<int, object> UserImages { get; set; }

        public Message NewestMessage { get; set; }
        public int? LastSeenMessageID { get; set; }

        public enum ConversationType
        {
            Profile, Group, Event
        }
    }
}