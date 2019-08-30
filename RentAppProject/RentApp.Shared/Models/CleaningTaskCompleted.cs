using System;
namespace RentAppProject
{
    public class CleaningTaskCompleted
    {
        public int ID { get; set; }
        public int CleaningTaskID { get; set; }
        public DateTime CompletedDate { get; set; }
        public string Comment { get; set; }
        public bool Confirmed { get; set; }
        public CleaningTask CleaningTask { get; set; }
    }
}
