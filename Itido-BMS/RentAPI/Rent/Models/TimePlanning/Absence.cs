using System;
using System.Linq.Expressions;

namespace Rent.Models.TimePlanning
{
    public class Absence
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public bool IsRequest { get; set; }
        public int CreatorID { get; set; }
        public int AbsenceReasonID { get; set; }
        public string Comment { get; set; }

        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public int RequestID { get; set; }

        public virtual User User { get; set; }
        public virtual User Creator { get; set; }
        public virtual AbsenceReason AbsenceReason { get; set; }
        public virtual Request Request { get; set; }

        public static Expression<Func<Absence, dynamic>> BasicDTO(int requester, string requesterRole)
        {
            return a => a != null ?
            new
            {
                a.ID,
                a.IsRequest,
                a.From,
                a.To,
                AbsenceReason = AbsenceReason.BasicDTO().Compile()(a.AbsenceReason),
                Request = (a.Request != null && a.Request.Creator != null && a.Request.Subject != null) ? Request.BasicDTO(requester, requesterRole).Compile()(a.Request, a.Request.Creator) : null,
                User = User.BasicDTO().Compile()(a.User),
                Creator = User.BasicDTO().Compile()(a.Creator),
            } : null;
        }


        public static Expression<Func<Absence, dynamic>> StandardDTO(int requester, string requesterRole)
        {
            return a => a != null ?
            new
            {
                a.ID,
                a.IsRequest,
                a.From,
                a.To,
                AbsenceReason = AbsenceReason.BasicDTO().Compile()(a.AbsenceReason),
                Request = (a.Request != null && a.Request.Creator != null && a.Request.Subject != null) ? Request.BasicDTO(requester, requesterRole).Compile()(a.Request, a.Request.Creator) : null,
                User = User.BasicDTO().Compile()(a.User),
                Creator = User.BasicDTO().Compile()(a.Creator),
            } : null;
        }
    }
}