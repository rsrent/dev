using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models
{
    public class RatingItem
    {
        public int ID { get; set; }
        public int RatingID { get; set; }
        [ForeignKey("User")]
        public int RatedByID { get; set; }

        public string Title { get; set; }
        public int Value { get; set; }
        public string Comment { get; set; }

        public DateTime TimeRated { get; set; }

        public virtual User User { get; set; }
        public virtual Rating Rating { get; set; }
    }
}
