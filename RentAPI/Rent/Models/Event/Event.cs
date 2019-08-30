using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models.Event
{
    public class Event
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [ForeignKey("Creator")]
		public int CreatorID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedTime { get; set; }

        public virtual User Creator { get; set; }
    }
}
