using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Message
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int ConversationID { get; set; }
        public string MessageText { get; set; }
        public DateTime SentTime { get; set; }
        public int? SpecialMessageID { get; set; }
        public string Type { get; set; }

        public virtual User User { get; set;}
        public virtual Conversation Conversation { get; set; }
        public virtual SpecialMessage SpecialMessage { get; set; }
    }

    public enum Status
    {
        Awaiting, Confirmed, Declined
    }
}
