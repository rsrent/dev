using System;
using System.Collections.Generic;
using System.Linq;

namespace RentAppProject
{
    public class QualityReport
    {
        public int ID { get; set; }
        public int UserID { get; set; }
		public int LocationID { get; set; }
		public DateTime Time { get; set; }
        public DateTime? CompletedTime { get; set; }
		//public List<QualityReportItem> QualityReportItems { get; set; }
        //public Dictionary<string, Dictionary<string, List<QualityReportItem>>> Plan;

        public ICollection<QualityReportFloor> Floors { get; set; }
    }

    public class QualityReportFloor 
    {
        public Floor Floor { get; set; }
        public ICollection<QualityReportArea> Areas { get; set; }
    }

    public class QualityReportArea
    {
        public Area Area { get; set; }
        public ICollection<QualityReportItem> QualityReportItems { get; set; }
    }
}