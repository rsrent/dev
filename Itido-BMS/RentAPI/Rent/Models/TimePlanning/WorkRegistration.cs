using System;
using System.Linq.Expressions;
namespace Rent.Models.TimePlanning
{
    public class WorkRegistration
    {
        public int ID { get; set; }
        public int WorkID { get; set; }
        public DateTime Date { get; set; }
        public int UserID { get; set; }
        public short? StartTimeMins { get; set; }
        public short? EndTimeMins { get; set; }
        public short? BreakMins { get; set; }
        public int? RequestID { get; set; }

        public virtual Work Work { get; set; }

        public virtual Request Request { get; set; }
        public virtual User User { get; set; }

        public static Expression<Func<WorkRegistration, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.WorkID,
                v.Date,
                v.StartTimeMins,
                v.EndTimeMins,
                //Request = (v.Request != null && v.Request.Creator != null && v.Request.Subject != null) ? Request.BasicDTO(requester, requesterRole).Compile()(v.Request, v.Request.Creator) : null,
            } : null;
        }

    }


}