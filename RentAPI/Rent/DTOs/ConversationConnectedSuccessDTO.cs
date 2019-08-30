using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class ConversationConnectedSuccessDTO
    {
        public string Text { get { return "connected"; } }
        public int ConversationID { get; set; }
        public string Type { get { return "Connect"; } }
    }
}
