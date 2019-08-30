using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models.Event
{
    public class EventParticipant
    {
        public int ID { get; set; }
        public int EventID { get; set; }
        public int UserID { get; set; }
        [ForeignKey("InvitedBy")]
        public int InvitedByID { get; set; }
        public DateTime InvitedTime { get; set; }
        public EventParticipationStatus Status { get; set; }

        public virtual Event Event { get; set; }
        public virtual User User { get; set; }
        public virtual User InvitedBy { get; set; }
    }
}
