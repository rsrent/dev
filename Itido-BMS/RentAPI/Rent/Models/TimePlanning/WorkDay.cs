
namespace Rent.Models.TimePlanning

{
    public class WorkDay
    {
        public int WorkContractID { get; set; }
        public int DayOfWeek { get; set; }
        public byte IsEvenWeek { get; set; }
        public short StartTimeMins { get; set; }
        public short EndTimeMins { get; set; }
        public short BreakMins { get; set; }
        public virtual WorkContract WorkContract { get; set; }
    }
}