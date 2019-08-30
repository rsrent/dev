using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    /*
    public class QualityReportDTO
    {
        public int ID { get; set; }
        public DateTime Time { get; set; }
        public DateTime? CompletedTime { get; set; }
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public int? RatingID { get; set; }

        //public virtual Location Location { get; set; }
        //public virtual User User { get; set; }
        //public virtual Rating Rating { get; set; }
        public ICollection<QualityReportItemDTO> QualityReportItems { get; set; }

        void SetupBasics(QualityReport q) {
            ID = q.ID;
            Time = q.Time;
            CompletedTime = q.CompletedTime;
            UserID = q.UserID;
            LocationID = q.LocationID;
            RatingID = q.RatingID;
        }

        public QualityReportDTO() { }

        public QualityReportDTO(QualityReport qualityReport)
        {
            ID = qualityReport.ID;
            Time = qualityReport.Time;
            CompletedTime = qualityReport.CompletedTime;
            UserID = qualityReport.UserID;
            LocationID = qualityReport.LocationID;
            RatingID = qualityReport.RatingID;
            QualityReportItems = qualityReport.QualityReportItems.Select(i => new QualityReportItemDTO(i)).ToList();
        }
    }
    */
}
