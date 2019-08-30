using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Area
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public int? TranslationID { get; set; }

        public int CleaningPlanID { get; set; }
        public virtual CleaningPlan CleaningPlan { get; set; }
    }

    public static class AreaDto {
        
        public static dynamic Basic(this Area a)
        {
            return new
            {
                a.Description, a.CleaningPlanID
            };
        }
    }
}
