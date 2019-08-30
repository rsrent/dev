using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models.Event
{
    public class Work
    {
        public int ID { get; set; }
        public int EventID { get; set; }

        [ForeignKey("StartGeoPosition")]
        public int? StartGeoPositionID { get; set; }
        [ForeignKey("EndGeoPosition")]
        public int? EndGeoPositionID { get; set; }

        public string Comment { get; set; }
        [ForeignKey("ConfirmingUser")]
        public int? ConfirmingUserID { get; set; }
        public bool WorkConfirmed { get; set; }

        public int? CleaningTaskID { get; set; }
        public int? LocationID { get; set; }

        public virtual Event Event { get; set; }
        public virtual User ConfirmingUser { get; set; }

        public virtual GeoPosition StartGeoPosition { get; set; }
        public virtual GeoPosition EndGeoPosition { get; set; }
    }
}
