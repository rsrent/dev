using System;
using Rent.Models;

namespace Rent.DTOs
{
    /*
    public class CleaningTaskDTO
    {
		public int ID { get; set; }
		//public Plan PlanType { get; set; }
		public int SquareMeters { get; set; }
		public string Frequency { get; set; }
        public int LocationID { get; set; }
		public byte? TimesOfYear { get; set; }
		public string Comment { get; set; }
        public DateTime? FirstCleaned { get; set; }
        public DateTime NextCleaned { get; set; }
		public Floor Floor { get; set; }
		public Area Area { get; set; }
        public CleaningPlan CleaningPlan { get; set; }

        public CleaningTaskCompleted LastTaskCompleted { get; set; }
        public int TimesCleanedThisYear { get; set; }


        public LocationDTO Location { get; set; }

        public CleaningTaskDTO() {}

        public CleaningTaskDTO(CleaningTask.DB ct, int timesCleanedThisYear, CleaningTaskCompleted lastCleaningTaskCompleted)
        {
            Area = ct.Area;
            Floor = ct.Floor;
            CleaningPlan = ct.Area.CleaningPlan;
            //TimesCleanedThisYear = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID && ctc.CompletedDate.Year == DateTime.Now.Year).Count(),
            //LastTaskCompleted = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID).OrderByDescending(ctc => ctc.CompletedDate).FirstOrDefault(),
            Comment = ct.Comment;
            TimesOfYear = CalculateTimesOfYear(ct.TimesOfYear, ct.FirstCleaned);
            SquareMeters = ct.SquareMeters;
            Frequency = ct.Frequency;
            FirstCleaned = ct.FirstCleaned;
            ID = ct.ID;
        }

        byte? CalculateTimesOfYear(byte? timesOfYear, DateTime? firstCleaned) {
            if (timesOfYear != null && firstCleaned != null && firstCleaned.Value.Year == DateTime.UtcNow.Year)
            {
                float interval = 365.0f / ((float)timesOfYear);
                int periodsPriorToFirstCleanedDate =
                    Math.Max((int)(((float)firstCleaned.Value.DayOfYear) / interval), 0);
                return (byte) (timesOfYear - periodsPriorToFirstCleanedDate);
            }
            else
            {
                return timesOfYear;
            }
        } 
    }
    */
}
