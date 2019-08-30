using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class ConversationUsers
    {
        public int UserID { get; set; }
        public int ConversationID { get; set; }

        public bool NotificationsOn { get; set; }

        [ForeignKey("LastSeenMessage")]
        public int? LastSeenMessageID { get; set; }

        public int UnseenMessages { get; set; }

        public virtual User User { get; set;}
        public virtual Conversation Conversation { get; set; }
        public virtual Message LastSeenMessage { get; set; }
    }
}
