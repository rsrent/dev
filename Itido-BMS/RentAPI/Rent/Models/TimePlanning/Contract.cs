using System;
using Rent.DTOs.TimePlanningDTO;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rent.Helpers;

namespace Rent.Models.TimePlanning
{

    public class Contract
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int AgreementID { get; set; }
        public float WeeklyHours { get; set; }
        public DateTime From { get; set; }
        public DateTime To { get; set; }
        public virtual Agreement Agreement { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<WorkContract> WorkContracts { get; set; }
        public virtual ICollection<WorkInvitation> WorkInvitations { get; set; }

        public virtual ICollection<Work> Work { get; set; }
        public virtual ICollection<WorkReplacement> WorkReplacements { get; set; }

        public static Expression<Func<Contract, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                User = User.BasicDTO().Compile()(v.User)
            } : null;
        }


        public static Expression<Func<Contract, dynamic>> DetailedDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.WeeklyHours,
                v.From,
                v.To,
                User = User.BasicDTO().Compile()(v.User),
                Agreement = Agreement.BasicDTO().Compile()(v.Agreement)

            } : null;
        }
    }
}
