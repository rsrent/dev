using System.Collections.Generic;
using System.Linq.Expressions;
using System;


namespace Rent.Models.TimePlanning
{

    public class Agreement
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Contract> Contracts { get; set; }


        public static Expression<Func<Agreement, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Name

            } : null;
        }

    }

}
