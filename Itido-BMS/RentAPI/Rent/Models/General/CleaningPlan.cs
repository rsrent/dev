using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class CleaningPlan
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public bool HasFloors { get; set; }
        public int? TranslationID { get; set; }
    }

    public static class CleaningPlanDto
    {
        public static dynamic Basic(this CleaningPlan c)
        {
            return new
            {
                c.ID,
                c.Description,
                c.HasFloors
            };
        }
    }
}
