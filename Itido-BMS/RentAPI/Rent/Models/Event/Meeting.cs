using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models.Event
{
    public class Meeting
    {
        public int ID { get; set; }
        public int EventID { get; set; }

        [ForeignKey("GeoPosition")]
        public int? GeoPositionID { get; set; }

        public virtual Event Event { get; set; }
        public virtual GeoPosition GeoPosition { get; set; }
    }
}
