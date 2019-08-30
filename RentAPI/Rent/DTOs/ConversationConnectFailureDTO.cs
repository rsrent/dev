using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class ConversationConnectFailureDTO
    {
        public string Text { get; set; }
        public int ConversationID { get; set; }
        public int UserID { get; set; }
        public string Type { get { return "Connect"; } }
    }
}
