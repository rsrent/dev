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
		public List<QualityReportItem> QualityReportItems { get; set; }
        public Dictionary<string, Dictionary<string, List<QualityReportItem>>> Plan;
    }
}