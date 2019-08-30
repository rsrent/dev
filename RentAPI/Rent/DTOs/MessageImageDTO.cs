using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class MessageImageDTO : MessageDTO
    {
        public int ConversationID { get; set; }
        public string ImageLocator { get; set; }

        public override sealed string Type { get { return "message_image"; } }
    }
}
