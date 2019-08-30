using System.Collections.Generic;


namespace Rent.Models.TimePlanning
{
    public class Holiday
    {
        public string Name { get; set; }
        public string CountryCode { get; set; }
        public virtual ICollection<WorkHoliday> WorkHolidays { get; set; }
    }
}