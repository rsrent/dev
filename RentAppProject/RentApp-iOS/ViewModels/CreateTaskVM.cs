using System;
using RentAppProject;
using UIKit;

namespace RentApp.ViewModels
{
    public class CreateTaskVM
    {
        public QualityReport QualityReport { get; set; }
        public Location Location { get; set; }
        public UIViewController RootViewController { get; set; }
        public UIViewController SameAreaViewController { get; set; }
        public CleaningTask Task { get; set; }

		public CleaningPlan CleaningPlan { get; set; }
        public Floor Floor { get; set; }
        public Area Area { get; set; }
    }
}
