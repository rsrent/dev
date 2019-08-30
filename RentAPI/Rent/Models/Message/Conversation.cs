using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Conversation
    {
        public int ID { get; set; }
        public string Title { get; set; }

        public bool Open { get; set; }

        [ForeignKey("NewestMessage")]
        public int? NewestMessageID { get; set; }
        public virtual Message NewestMessage { get; set; }

        /*
        [ForeignKey("Customer")]
        public int? CustomerID { get; set; }

        [ForeignKey("CustomerLocation")]
        public int? CustomerLocationID { get; set; }

        [ForeignKey("TeamLocation")]
        public int? TeamLocationID { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Location CustomerLocation { get; set; }
        public virtual Location TeamLocation { get; set; }
        */
    }
}
