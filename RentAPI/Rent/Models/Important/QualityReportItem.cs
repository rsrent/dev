using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories;

namespace Rent.Models
{
    public class QualityReportItem
    {
        public int ID { get; set; }
        public int CleaningTaskID { get; set; }
        public byte Rating { get; set; }
        public int QualityReportID { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }

        public virtual CleaningTask CleaningTask { get; set; }
        public virtual QualityReport QualityReport { get; set; }
    }

    public static class Dto
    {
        public static object Standard(this QualityReportItem q)
        {
            if (q == null) return null;


            return Merger.Merge(new
            {

            }, q.Basic());
        }

        public static object Basic(this QualityReportItem q)
        {
            if (q == null) return null;

            return new
            {
                q.ID,
                q.CleaningTaskID,
                q.Rating,
                q.QualityReportID,
                q.Comment,
                q.ImageLocation,
                CleaningTask = q.CleaningTask.Basic(),
            };
        }
    }

}
