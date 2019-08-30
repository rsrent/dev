using System;

namespace RentAppProject
{
    public class CleaningTask
    {
        public int ID { get; set; }
        public int SquareMeters { get; set; }
		public string Frequency { get; set; }
        public int LocationID { get; set; }
        public byte? TimesOfYear { get; set; }
		public DateTime? FirstCleaned { get; set; }
		public string Comment { get; set; }
		
        public Area Area { get; set; }
        public Floor Floor { get; set; }
        //public CleaningPlan CleaningPlan { get; set; }

		public CleaningTaskCompleted LastTaskCompleted { get; set; }
		public int TimesCleanedThisYear { get; set; }
    }
}
