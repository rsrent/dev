using System;
using System.Collections.Generic;
using Rent.Models.Projects;
using Rent.Repositories;

namespace Rent.Models
{
    public class QualityReport : IDto
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public DateTime? CompletedTime { get; set; }
        public int UserID { get; set; }
        public int? LocationID { get; set; }
        public int? RatingID { get; set; }

        public string Comment { get; set; }

        public virtual Location Location { get; set; }
        public virtual User User { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual ICollection<QualityReportItem> QualityReportItems { get; set; }
        public DateTime? NextReport { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }

        public override object Basic()
        {
            return new
            {
                ID,
                Time,
                CompletedTime,
                UserID,
                LocationID,
                RatingID
            };
        }
    }
}
