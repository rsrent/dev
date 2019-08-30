using System;
using Rent.Models.TimePlanning;
using System.Collections.Generic;

namespace Rent.DTOs.TimePlanningDTO
{

    public class WorkContractListDTO
    {
        public int ID { get; set; }
        public virtual UserListDTO User { get; set; }
        public int? LocationID { get; set; }

        public virtual Contract Contract { get; set; }
        public virtual ICollection<WorkDay> WorkDays { get; set; }
        public virtual ICollection<WorkHoliday> Holidays { get; set; }



        public string Note { get; set; }
        public dynamic Location { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }


    }
}
