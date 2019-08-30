using System;
namespace Rent.Models
{
    public class MoreWork
    {
        public int ID { get; set; }

        //person to complete the task
        public int? UserID { get; set; }
        public int LocationID { get; set; }
        public Location Location { get; set; }

        //user who ordered the work
        public int CreatedByUserID { get; set; }

        public string Description { get; set; }
        public int CleaningPlanID { get; set; }

        public float? Hours { get; set; }

        public DateTime ExpectedCompletedTime { get; set; }
        public DateTime? ActualCompletedTime { get; set; }
    }
}
