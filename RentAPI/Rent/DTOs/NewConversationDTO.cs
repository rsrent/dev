using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class NewConversationDTO
    {
        public string Title { get; set; }
        public ICollection<Int32> UserIDs { get; set; }
    }
}
