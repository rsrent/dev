using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class MessageTextDTO : MessageDTO
    {
        public MessageTextDTO(Rent.Models.Message message)
        {
            MessageText = message.MessageText;
            ConversationID = message.ConversationID;
        }

        public MessageTextDTO() { }
        public string MessageText { get; set; }
        public int ConversationID { get; set; }

        public override sealed string Type { get { return "Text"; } }
    }
}
