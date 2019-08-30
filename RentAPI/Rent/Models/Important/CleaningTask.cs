using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Rent.Repositories;

namespace Rent.Models
{
    public class CleaningTask : IDto
    {
        public int ID { get; set; }
        public int? FloorID { get; set; }
        public int AreaID { get; set; }
        public int SquareMeters { get; set; }
        public int LocationID { get; set; }
        public string Frequency { get; set; }
        public byte? TimesOfYear { get; set; }
        public DateTime? FirstCleaned { get; set; }
        //public DateTime? LastCleaned { get; set; }
        public string Comment { get; set; }
        public int? UserResposibleID { get; set; }
        public bool Active { get; set; }
        public int? LastTaskCompletedID { get; set; }
        
        public virtual Floor Floor { get; set; }
        public virtual Area Area { get; set; }
        public virtual User UserResposible { get; set; }
        public virtual Location Location { get; set; }
        public virtual ICollection<CleaningTaskCompleted> CompletedTasks { get; set; }
        public virtual CleaningTaskCompleted LastTaskCompleted { get; set; }
        
        public override object Detailed()
        {
            DateTime NextCleaned = new DateTime();
            byte? TimesThisYear = 0;

            var LastTaskCompleted = CompletedTasks?.OrderByDescending(ctc => ctc.CompletedDate).FirstOrDefault();

            if (TimesOfYear != null)
            {
                NextCleaned = LastTaskCompleted?.CompletedDate.AddDays((int)(365.0 / (int)TimesOfYear)) ?? new DateTime();
                TimesThisYear = CalculateTimesOfYear(TimesOfYear, FirstCleaned);
            }

            var thisYear = DateTime.UtcNow.Year;
            var TimesCleanedThisYear = CompletedTasks?.Count(ctc => ctc.CompletedDate.Year == thisYear) ?? 0;


            return Merger.Merge(new {
                UserResposible = UserResposible?.Basic(),
                Location = Location?.Basic(),
                LastTaskCompleted = LastTaskCompleted?.Basic(),
                NextCleaned,
                TimesThisYear,
                TimesCleanedThisYear,
            }, Basic());
        }
        
        public override object Basic()
        {
            return new
            {
                ID,
                FloorID,
                AreaID,
                SquareMeters,
                LocationID,
                Frequency,
                TimesOfYear,
                FirstCleaned,
                Comment,
                UserResposibleID,
                Active,
                Floor = Floor?.Basic(),
                Area = Area?.Basic(),
            };
        }
        
        static byte? CalculateTimesOfYear(byte? timesOfYear, DateTime? firstCleaned)
        {
            if (timesOfYear != null && firstCleaned != null && firstCleaned.Value.Year == DateTime.UtcNow.Year)
            {
                var interval = 365.0f / ((float)timesOfYear);
                var periodsPriorToFirstCleanedDate =
                    Math.Max((int)(((float)firstCleaned.Value.DayOfYear) / interval), 0);
                return (byte)(timesOfYear - periodsPriorToFirstCleanedDate);
            }
            else
            {
                return timesOfYear;
            }
        }
    }

    public static class CleaningTaskDto
    {
        
    }

    public enum Plan { Regular, Window, FanCoil }
}
