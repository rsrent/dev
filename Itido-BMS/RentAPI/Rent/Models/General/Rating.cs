using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rent.Models
{
    public class Rating
    {
        public int ID { get; set; }
        public string Title { get; set; }


        public virtual ICollection<RatingItem> RatingItems { get; set; }
    }
}
