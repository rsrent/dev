using System;
using System.Linq.Expressions;
using Rent.Helpers;

namespace Rent.Models.TimePlanning
{
    public class WorkReplacement
    {
        public int ID { get; set; }
        public int WorkID { get; set; }
        public int? ContractID { get; set; }
        public int AbsenceID { get; set; }
        public virtual Contract Contract { get; set; }
        public virtual Absence Absence { get; set; }
        public virtual Work Work { get; set; }


        public static Expression<Func<WorkReplacement, dynamic>> DetailedDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                //Absence = Absence.BasicDTO(requester, requesterRole).Compile()(v.Absence),
                //AbsenceReason = v.Absence != null ? v.Absence.AbsenceReason : null,
                //Contract = Contract.BasicDTO().CallWithArg(v.Contract),

            } : null;
        }

        public static Expression<Func<WorkReplacement, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                //AbsenceReason = v.Absence != null ? v.Absence.AbsenceReason : null,
                //Contract = Contract.BasicDTO().CallWithArg(v.Contract),
            } : null;
        }
    }
}