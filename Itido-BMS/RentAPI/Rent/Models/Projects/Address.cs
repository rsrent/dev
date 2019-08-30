using System;
using System.Linq.Expressions;

namespace Rent.Models.Projects
{
    public class Address
    {
        public int ID { get; set; }
        public int ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public string AddressName { get; set; }
        public double? Lat { get; set; }
        public double? Lon { get; set; }

        static public Expression<Func<Address, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.AddressName,
                v.Lat,
                v.Lon,
            } : null;
        }
    }
}
