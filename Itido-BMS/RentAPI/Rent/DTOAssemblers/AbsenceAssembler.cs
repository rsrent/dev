using Rent.Repositories.TimePlanning;
using Rent.DTOs.TimePlanningDTO;
using Rent.Models.TimePlanning;
using Rent.Models;
using Rent.Data;
using System.Collections.Generic;
using System;

namespace Rent.DTOAssemblers
{
    public class AbsenceAssembler
    {

        /*
        private readonly ApprovalStateRepository _approvalStateRepository;


        public AbsenceAssembler(ApprovalStateRepository approvalStateRepository)
        {
            _approvalStateRepository = approvalStateRepository;

        }
        */

        /*
            Functions for DTOs
         */

        public static System.Linq.Expressions.Expression<Func<User, dynamic>> UserDTO = u => new { u.FirstName };
        public static dynamic GetUser(User u) => new { u.FirstName };


        public System.Linq.Expressions.Expression<Func<Absence, dynamic>> AbsenceDTO(int requester, bool isRequesterAdminOrManager)
        {
            //return Absence.StandardDTO(requester, isRequesterAdminOrManager);
            return absence => absence != null ? new
            {
                absence.ID,
                absence.From,
                absence.To,
                absence.Comment,
                absence.IsRequest,
                AbsenceReason = new
                {
                    absence.AbsenceReason.ID,
                    absence.AbsenceReason.Description,
                },
                Creator = new
                {
                    absence.Creator.ID,
                    absence.Creator.FirstName,
                    absence.Creator.LastName,
                    absence.Creator.EmployeeNumber,
                },
                Request = absence.Request != null ? new
                {
                    absence.Request.ApprovalState,
                    CanRespondToApprovalState = absence.Request.ApprovalState == ApprovalState.Pending &&
                        (((absence.Request.Creator.UserRole.Equals("Admin") || absence.Request.Creator.UserRole.Equals("Manager")) && absence.Request.SubjectID == requester) ||
                        ((isRequesterAdminOrManager) && absence.Request.SubjectID == absence.Request.CreatorID)),

                } : null
            } : null;


            return TestExpression.CreateNewStatement("Creator");

            return absence => new
            {
                ID = absence.ID,
                //UserID = absence.UserID,
                //Description = absence.AbsenceReason != null ? absence.AbsenceReason.Description : null,
                From = absence.From,
                To = absence.To,
                Comment = absence.Comment,
                IsRequest = absence.IsRequest,
                AbsenceReason = new
                {
                    absence.AbsenceReason.ID,
                    absence.AbsenceReason.Description,
                },
                Creator = new
                {
                    absence.Creator.ID,
                    absence.Creator.FirstName,
                    absence.Creator.LastName,
                    absence.Creator.EmployeeNumber,
                },
                Request = absence.Request != null ? new
                {
                    absence.Request.ApprovalState,
                    CanRespondToApprovalState = absence.Request.ApprovalState == ApprovalState.Pending &&
                        (((absence.Request.Creator.UserRole.Equals("Admin") || absence.Request.Creator.UserRole.Equals("Manager")) && absence.Request.SubjectID == requester) ||
                        ((isRequesterAdminOrManager) && absence.Request.SubjectID == absence.Request.CreatorID)),

                } : null
            };
        }
        /*
        public System.Linq.Expressions.Expression<Func<Absence, Object>> AbsenceDTO = absence => new {
                ID = absence.ID,
                UserID = absence.UserID,
                Description = absence.AbsenceReason != null? absence.AbsenceReason.Description : null,
                From = absence.From,
                To = absence.To,
                Comment = absence.Comment,
                ApprovalState = absence.ApprovalState,
                IsRequest = absence.IsRequest,
                CanRespondToApprovalState = absence.ApprovalState == ApprovalState.Pending ? (
                    (absence.Creator.UserRole.Equals("Admin") || absence.Creator.UserRole.Equals("Manager")) && absence.UserID == requester
                ) : false
            // CanRespondToApprovalState = true, //_approvalStateRepository.CanRequesterReplyToApprovalState(requester), //TODO send rigtig info

            //User = absence.User != null? new 
            //{
            //    ID = absence.User.ID,
            //    FirstName = absence.User.FirstName,
            //    LastName = absence.User.LastName,
            //    EmployeeNumber = absence.User.EmployeeNumber,
            //    RoleName = absence.User.Role!=null? absence.User.Role.Name: null,

            //} : null,
            //Creator = absence.Creator != null? new 
            //{

            //    ID = absence.Creator.ID,
            //    FirstName = absence.Creator.FirstName,
            //    LastName = absence.Creator.LastName,
            //    EmployeeNumber = absence.Creator.EmployeeNumber,
            //    RoleName = absence.Creator.Role!=null? absence.Creator.Role.Name: null,

            //}:null

        };
        */

        /*
            Functions for DTOs
         */


        public Absence CreateAbsenceFromDTO(int requester, AbsenceDTO absenceDTO)
        {
            return new Absence
            {
                UserID = absenceDTO.UserID,
                AbsenceReasonID = absenceDTO.AbsenceReasonID,
                From = absenceDTO.From,
                To = absenceDTO.To,
                Comment = absenceDTO.Comment,

                CreatorID = requester,
                IsRequest = absenceDTO.IsRequest,
            };

        }


        /* 
        public AbsenceDTO CreateAbsenceDTO(int requester, Absence absence)
        {
            Console.WriteLine("Got to before can reply call");

            var canReply = _approvalStateRepository.CanRequesterReplyToApprovalState(requester, absence.ApprovalState, absence.CreatorID, absence.UserID);
            Console.WriteLine("Got to after can reply call");
            return new AbsenceDTO{
                ID = absence.ID,
                UserID =  absence.UserID,
                AbsenceReasonID = absence.AbsenceReasonID,
                Comment = absence.Comment,
                Description = absence.AbsenceReason?.Description,
                From = absence.From,
                To = absence.To,
                CanRespondToApprovalState = canReply,
                CreatorName = absence.Creator?.FirstName + " " + absence.Creator?.LastName,
                ApprovalState = absence.ApprovalState,
                IsRequest = absence.IsRequest,
            };
        }

         public IEnumerable<AbsenceDTO> CreateAbsenceDTOsList(int requester, IEnumerable<Absence> absences)
        {
            var absenceDTOs = new List<AbsenceDTO>();
            foreach(var absence in absences)
            {
                Console.WriteLine("Inside create absence dto loop");

                absenceDTOs.Add(CreateAbsenceDTO(requester, absence));

            }
            return absenceDTOs;
        }
    */

    }
}