using System;
using System.Collections.Generic;
using System.Linq;

namespace RentAppProject
{
    public class CleaningSchedule
    {
        public int ID { get; set; }
        public int LocationID { get; set; }

        public List<SchedulePlan> Schedule { get; set; }

        public class SchedulePlan
        {
            public CleaningPlan CleaningPlan { get; set; }
            public ICollection<ScheduleFloor> Floors { get; set; }
            public ICollection<ScheduleArea> Areas { get; set; }
        }

        public class ScheduleFloor {
            public Floor Floor { get; set; }
            public ICollection<ScheduleArea> Areas { get; set; }
        }

        public class ScheduleArea
        {
            public Area Area { get; set; }
            public ICollection<CleaningTask> Tasks { get; set; }
        }
    }
}
