using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories;

namespace Rent.Models
{
    public class CleaningTaskCompleted
    {
        public int ID { get; set; }
        public int CleaningTaskID { get; set; }
        public DateTime CompletedDate { get; set; }
        public string Comment { get; set; }
        public bool Confirmed { get; set; }
        public int CompletedByUserID { get; set; }
        
        public virtual CleaningTask CleaningTask { get; set; }
        public virtual User CompletedByUser { get; set; }
    }

    public static class CleaningTaskCompletedDto
    {
        public static object Standard(this CleaningTaskCompleted ctc)
        {
            if (ctc == null) return null;
            
            return Merger.Merge(new { }, ctc.Basic());
        }
        
        public static object Basic(this CleaningTaskCompleted ctc)
        {
            if (ctc == null) return null;
            return new
            {
                ctc.ID,
                ctc.CleaningTaskID,
                ctc.CompletedDate,
                ctc.Comment,
                ctc.Confirmed,
                ctc.CompletedByUserID,
                CleaningTask = ctc.CleaningTask?.Basic(),
                CompletedByUser = ctc.CompletedByUser?.Basic(),
            };
        }
    }
}
