using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs.TimePlanningDTO
{
    public class ContractDTO
    {
        public int ID { get; set; }
        public string AgreementName { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public float WeeklyHours { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
