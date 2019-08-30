using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class ConversationConnectDTO : MessageDTO
    {
        public override sealed string Type { get { return "connect"; } }
    }
}
