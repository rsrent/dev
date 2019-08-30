using System;
using System.Linq;
using System.Linq.Expressions;
using Rent.Models.Projects;
using System.Collections.Generic;
using Rent.Helpers;

namespace Rent.Models.TimePlanning
{
    public class Work
    {
        public int ID { get; set; }
        public int? ContractID { get; set; }
        public int? WorkContractID { get; set; }

        //public int? LocationID { get; set; }

        public string Note { get; set; }
        public DateTime Date { get; set; }

        public int Modifications { get; set; }

        public short StartTimeMins { get; set; }
        public short EndTimeMins { get; set; }
        public short BreakMins { get; set; }
        //public int? WorkReplacementID { get; set; }
        public bool IsVisible { get; set; }
        // public int? WorkRegistrationID { get; set; }
        public virtual Contract Contract { get; set; }
        //public virtual Location Location { get; set; }
        public virtual WorkReplacement WorkReplacement { get; set; }
        public virtual WorkRegistration WorkRegistration { get; set; }

        public virtual ICollection<WorkInvitation> WorkInvitations { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }

        public static Expression<Func<Work, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.Date,
                v.StartTimeMins,
                v.EndTimeMins,
                v.BreakMins,
                v.IsVisible,
            } : null;
        }

        public static Expression<Func<Work, dynamic>> StandardDTO(int requester, string requesterRole)
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.Date,
                v.StartTimeMins,
                v.EndTimeMins,
                v.BreakMins,
                v.IsVisible,

                // project 
                ProjectItem = ProjectItem.BasicDTO().Compile()(v.ProjectItem),
                ProjectItem_project = v.ProjectItem != null ? Project.BasicDTO().Compile()(v.ProjectItem.Project) : null,

                // contract
                Contract = Contract.BasicDTO().Compile()(v.Contract),
                Contract_user = v.Contract != null ? User.BasicDTO().Compile()(v.Contract.User) : null,

                // workReplacement
                WorkReplacement =
                    WorkReplacement.BasicDTO().Compile()(v.WorkReplacement),
                WorkReplacement_contract =
                    v.WorkReplacement != null ? Contract.BasicDTO().Compile()(v.WorkReplacement.Contract) : null,
                WorkReplacement_contract_user =
                    v.WorkReplacement != null && v.WorkReplacement.Contract != null ? User.BasicDTO().Compile()(v.WorkReplacement.Contract.User) : null,
                WorkReplacement_absence = Absence.BasicDTO(requester, requesterRole).Compile()(v.WorkReplacement.Absence),
                WorkReplacement_absence_absenceReason = AbsenceReason.BasicDTO().Compile()(v.WorkReplacement.Absence.AbsenceReason),

                // workRegistration
                WorkRegistration = WorkRegistration.BasicDTO().Compile()(v.WorkRegistration),
                WorkRegistration_request = Request.BasicDTO(requester, requesterRole).Compile()(v.WorkRegistration.Request, v.WorkRegistration.Request.Creator),

                IsInvited = v.WorkInvitations.Any(inv => inv.Contract.UserID == requester),
                InviteCount = v.WorkInvitations.Count,

                CanRegisterWork = v.WorkRegistration == null && (requesterRole == "Admin" || (v.Contract != null && (v.WorkReplacement != null && v.WorkReplacement.Contract != null && v.WorkReplacement.Contract.UserID == requester) || (v.WorkReplacement == null && v.Contract.UserID == requester))),
            } : null;
        }

        public static Expression<Func<Work, dynamic>> DetailedDTO(int requester, string requesterRole)
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Note,
                v.Date,
                v.StartTimeMins,
                v.EndTimeMins,
                v.BreakMins,
                v.IsVisible,

                // project 

                ProjectItem = ProjectItem.BasicDTO().Compile()(v.ProjectItem),
                ProjectItem_project = v.ProjectItem != null ? Project.BasicDTO().Compile()(v.ProjectItem.Project) : null,

                // contract
                Contract = Contract.BasicDTO().Compile()(v.Contract),
                Contract_user = v.Contract != null ? User.BasicDTO().Compile()(v.Contract.User) : null,

                // workReplacement
                WorkReplacement =
                    WorkReplacement.BasicDTO().Compile()(v.WorkReplacement),
                WorkReplacement_contract =
                    v.WorkReplacement != null ? Contract.BasicDTO().Compile()(v.WorkReplacement.Contract) : null,
                WorkReplacement_contract_user =
                    v.WorkReplacement != null && v.WorkReplacement.Contract != null ? User.BasicDTO().Compile()(v.WorkReplacement.Contract.User) : null,
                WorkReplacement_absence = v.WorkReplacement != null ? Absence.BasicDTO(requester, requesterRole).Compile()(v.WorkReplacement.Absence) : null,
                WorkReplacement_absence_absenceReason = AbsenceReason.BasicDTO().Compile()(v.WorkReplacement.Absence.AbsenceReason),

                // workRegistration
                WorkRegistration = WorkRegistration.BasicDTO().Compile()(v.WorkRegistration),
                WorkRegistration_request = v.WorkRegistration != null ? Request.BasicDTO(requester, requesterRole).Compile()(v.WorkRegistration.Request, v.WorkRegistration.Request.Creator) : null,

                IsInvited = v.WorkInvitations.Any(inv => inv.Contract.UserID == requester),
                InviteCount = v.WorkInvitations.Count,

                CanRegisterWork = v.WorkRegistration == null && (requesterRole == "Admin" || (v.Contract != null && (v.WorkReplacement != null && v.WorkReplacement.Contract != null && v.WorkReplacement.Contract.UserID == requester) || (v.WorkReplacement == null && v.Contract.UserID == requester))),
            } : null;
        }
    }
}