using System;
namespace Rent.Models.Important
{
    public class News
    {
        public int Id { get; set; }

        public string Title { get; set; }
        public string Body { get; set; }
        public DateTime Time { get; set; }

        public NewsCategory Category { get; set; }

        public int SubjectId { get; set; }

        public int UserId { get; set; }
        public int? LocationId { get; set; }

        public User User { get; set; }
        public Location Location { get; set; }

    }

    public enum NewsCategory
    {
        MoreWorkOrdered, MoreWorkCompleted, Other, QualityReportStarted, QualityReportCompleted, WindowTaskCompleted, FanCoilTaskCompleted
    }
}
