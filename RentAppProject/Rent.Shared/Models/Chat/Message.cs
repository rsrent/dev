using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace RentAppProject
{
    public abstract class Message
    {
        public abstract string Type { get; }
        public int ID { get; set; }
        public int UserId { get; set; }
        public int ConversationId { get; set; }
        public DateTime SentTime { get; set; }
        public string MessageText { get; set; }
    }
}
