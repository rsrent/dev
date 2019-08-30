using System;
using System.Collections.Generic;
using Rent.Models;

namespace Rent.DTOs
{
    public class ConversationDTO 
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public bool Open { get; set; }
        public Message NewestMessage { get; set; }
        public int? LastSeenMessageID { get; set; }
    }
}
