using System;
using Rent.Models.TimePlanning.Enums;

namespace Rent.Models.TimePlanning
{
    public class AccidentReport
    {
        public int ID { get; set; }
        public AccidentReportType AccidentReportType { get; set; }
        public DateTime DateTime { get; set; }
        public string Place { get; set; }
        public string Description { get; set; }
        public string ActionTaken { get; set; }
        public int AbsenceDurationDays { get; set; }

        public int? UserID { get; set; }
        public virtual User User { get; set; }

        public int? CreatorID { get; set; }
        public virtual User Creator { get; set; }

        public int RequestID { get; set; }
        public virtual Request Request { get; set; }
    }
}
