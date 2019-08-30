
namespace Rent.Models.TimePlanning
{

    public class WorkHoliday
    {
        public string HolidayName { get; set; }
        public string HolidayCountryCode { get; set; }
        public int WorkContractID { get; set; }
        public virtual Holiday Holiday { get; set; }
        public virtual WorkContract WorkContract { get; set; }
    }
}