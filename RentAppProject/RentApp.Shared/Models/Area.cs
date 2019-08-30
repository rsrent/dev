using System;
namespace RentAppProject
{
    public class Area
	{
		public int ID { get; set; }
		public string Description { get; set; }
        public CleaningPlan CleaningPlan { get; set; }
        public int CleaningPlanID { get; set; }
    }
}
