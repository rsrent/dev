using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Rent.Models.TimePlanning

{

    public class AbsenceReason
    {
        public int ID { get; set; }
        public string Description { get; set; }
        public bool CanUserCreate { get; set; }
        public bool CanUserRequest { get; set; }
        public bool CanManagerCreate { get; set; }
        public bool CanManagerRequest { get; set; }
        public virtual ICollection<Absence> Absence { get; set; }

        public static Expression<Func<AbsenceReason, dynamic>> BasicDTO()
        {
            return a => a != null ?
            new
            {
                a.ID,
                a.Description,
            } : null;
        }
    }
}