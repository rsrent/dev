
using System;
using Rent.Models.TimePlanning;

namespace Rent.DTOs.TimePlanningDTO

{
    public class WorkListDTO
    {
        public int ID { get; set; }
        public virtual UserListDTO User { get; set; }
        public string Note { get; set; }
        public DateTime Date { get; set; }
        public dynamic Location { get; set; }
        public int? LocationID { get; set; }

        public short StartTimeMins { get; set; }
        public short EndTimeMins { get; set; }
        public short BreakMins { get; set; }
        public virtual WorkReplacementDTO Replacement { get; set; }
        public bool IsVisible { get; set; }
        public virtual WorkRegistration WorkRegistration { get; set; }


    }
}