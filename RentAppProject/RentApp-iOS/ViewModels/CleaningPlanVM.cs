using System;
using System.Collections.Generic;
using RentAppProject;

namespace RentApp.ViewModels
{
    public class CleaningPlanVM
    {
        public Location Location { get; set; }

        public CleaningSchedule.ScheduleFloor ScheduleFloor { get; set; }
        public CleaningSchedule.SchedulePlan SchedulePlan { get; set; }

		public CleaningTask CleaningTask { get; set; }

        public CleaningSchedule CleaningSchedule { get; set; }
    }
}
