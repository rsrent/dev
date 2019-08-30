using Rent.DTOs.TimePlanningDTO;
using Rent.Models.TimePlanning;
using System.Collections.Generic;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Dynamic;
using Rent.Models;
using System.Reflection;

namespace Rent.DTOAssemblers
{
    public class WorkListDTOAssembler
    {
        UserListDTOAssembler _userListDTOAssembler;
        WorkReplacementDTOAssembler _workReplacementDTOAssembler;
        public WorkListDTOAssembler(UserListDTOAssembler userListDTOAssembler, WorkReplacementDTOAssembler workReplacementDTOAssembler)
        {
            _userListDTOAssembler = userListDTOAssembler;
            _workReplacementDTOAssembler = workReplacementDTOAssembler;
        }

        /*
            Function for DTOs
         */


        // o => new { o.ID }

        // CreateNewStatement("ID");

        
/*
        public System.Linq.Expressions.Expression<Func<Work, Object>> WorkBasic = work => new
        {
            ID = work.ID,
            Note = work.Note,
            Date = work.Date,
            StartTimeMins = work.StartTimeMins,
            EndTimeMins = work.EndTimeMins,
            BreakMins = work.BreakMins,
            IsVisible = work.IsVisible,
            Contract = work.Contract != null ? new
            {
                Id = work.Contract.ID,
                User = work.Contract.User != null ? new
                {
                    ID = work.Contract.User.ID,
                    FirstName = work.Contract.User.FirstName,
                    LastName = work.Contract.User.LastName,
                    EmployeeNumber = work.Contract.User.EmployeeNumber,
                    RoleName = work.Contract.User.Role != null ? work.Contract.User.Role.Name : null

                } : null
            } : null,

            WorkReplacement = work.WorkReplacement != null ? new
            {
                Absence = work.WorkReplacement.Absence != null ? new
                {
                    ID = work.WorkReplacement.Absence.ID,
                    Description = work.WorkReplacement.Absence.AbsenceReason != null ? work.WorkReplacement.Absence.AbsenceReason.Description : null,
                } : null,
                Contract = work.WorkReplacement.Contract != null ? new
                {
                    ID = work.WorkReplacement.Contract.ID,
                    User = work.WorkReplacement.Contract.User != null ?
                    new
                    {
                        FirstName = work.WorkReplacement.Contract.User.FirstName,
                        LastName = work.WorkReplacement.Contract.User.LastName,
                        EmployeeNumber = work.WorkReplacement.Contract.User.EmployeeNumber,
                        RoleName = work.WorkReplacement.Contract.User.Role != null ? work.WorkReplacement.Contract.User.Role.Name : null

                    } : null
                } : null,
            } : null,
            WorkRegistration = work.WorkRegistration != null ? new
            {
                ID = work.ID,
                Date = work.WorkRegistration.Date,
                StartTimeMins = work.WorkRegistration.StartTimeMins,
                EndTimeMins = work.WorkRegistration.EndTimeMins
            } : null,
            Location = work.Location != null ? new
            {
                ID = work.Location.ID,
                Name = work.Location.Name,
                CustomerName = work.Location.Customer != null ? work.Location.Customer.Name : null,
                ProjectNumber = work.Location.ProjectNumber,
                Address = work.Location.Address
            } : null,
            WorkContractID = work.WorkContractID,
        };


        public System.Linq.Expressions.Expression<Func<Work, Object>> WorkDetailed = work => new
        {
            ID = work.ID,
            Note = work.Note,
            Date = work.Date,
            StartTimeMins = work.StartTimeMins,
            EndTimeMins = work.EndTimeMins,
            BreakMins = work.BreakMins,
            IsVisible = work.IsVisible,
            Contract = work.Contract != null ? new
            {
                ID = work.Contract.ID,
                WeeklyHours = work.Contract.WeeklyHours,
                From = work.Contract.From,
                To = work.Contract.To,
                User =
                work.Contract.User != null ?
                   new
                   {
                       ID = work.Contract.User.ID,
                       FirstName = work.Contract.User.FirstName,
                       LastName = work.Contract.User.LastName,
                       EmployeeNumber = work.Contract.User.EmployeeNumber,
                       RoleName = work.Contract.User.Role != null ? work.Contract.User.Role.Name : null
                   } : null,

                Agreement = work.Contract.Agreement != null ?
                    new
                    {
                        ID = work.Contract.Agreement.ID,
                        Name = work.Contract.Agreement.Name
                    } : null
            } : null,
            WorkReplacement = work.WorkReplacement != null ? new
            {
                Contract = work.WorkReplacement.Contract != null ? new
                {
                    ID = work.WorkReplacement.Contract.ID,
                    User = work.WorkReplacement.Contract.User != null ?
                   new
                   {
                       FirstName = work.WorkReplacement.Contract.User.FirstName,
                       LastName = work.WorkReplacement.Contract.User.LastName,
                       EmployeeNumber = work.WorkReplacement.Contract.User.EmployeeNumber,
                       RoleName = work.WorkReplacement.Contract.User.Role != null ? work.WorkReplacement.Contract.User.Role.Name : null

                   } : null,
                    WeeklyHours = work.WorkReplacement.Contract.WeeklyHours,
                    From = work.WorkReplacement.Contract.From,
                    To = work.WorkReplacement.Contract.To,
                    Agreement = work.WorkReplacement.Contract.Agreement != null ?
                   new
                   {
                       ID = work.WorkReplacement.Contract.Agreement.ID,
                       Name = work.WorkReplacement.Contract.Agreement.Name
                   } : null

                } : null,
                Absence = work.WorkReplacement.Absence != null ? new
                {
                    ID = work.WorkReplacement.Absence.ID,
                    AbsenceReason = work.WorkReplacement.Absence.AbsenceReason != null ? work.WorkReplacement.Absence.AbsenceReason.Description : null,
                    From = work.WorkReplacement.Absence.From,
                    To = work.WorkReplacement.Absence.To,
                    Commment = work.WorkReplacement.Absence.Comment
                } : null
            } : null,
            WorkRegistration = work.WorkRegistration != null ? new
            {
                ID = work.ID,
                Date = work.WorkRegistration.Date,
                StartTimeMins = work.WorkRegistration.StartTimeMins,
                EndTimeMins = work.WorkRegistration.EndTimeMins
            } : null,
            Location = work.Location != null ? new
            {
                ID = work.Location.ID,
                Name = work.Location.Name,
                CustomerName = work.Location.Customer != null ? work.Location.Customer.Name : null,
                ProjectNumber = work.Location.ProjectNumber,
                Address = work.Location.Address
            } : null,
            WorkContractID = work.WorkContractID,

        };

 




        public WorkListDTO CreateWorkListDTO(Work work)
        {
            Console.WriteLine("Got to 1");
            UserListDTO user = null;
            if (work.Contract != null && work.Contract.User != null)
            {
                user = _userListDTOAssembler.CreateUserListDTO(work.Contract.User);
            }
            Console.WriteLine("Got to 2");
            WorkReplacementDTO replacement = null;
            if (work.WorkReplacement != null)
            {
                replacement = _workReplacementDTOAssembler.CreateWorkReplacementDTO(work.WorkReplacement);
            }
            Console.WriteLine("Got to 3");
            var location = work.Location;
            Console.WriteLine("Got to 4");
            return new WorkListDTO
            {
                ID = work.ID,
                User = user,
                Note = work.Note,
                LocationID = work.LocationID,
                Date = work.Date,
                StartTimeMins = work.StartTimeMins,
                EndTimeMins = work.EndTimeMins,
                BreakMins = work.BreakMins,
                Location = location == null ? null : new
                {
                    Name = location.Name,
                    CustomerName = location.Customer?.Name,
                    Address = location.Address
                },
                Replacement = replacement,
                IsVisible = work.IsVisible,
                WorkRegistration = work.WorkRegistration
            };
        }
        public IEnumerable<WorkListDTO> CreateWorkListDTOList(IQueryable<Work> works)
        {
            return works.Select(CreateWorkListDTO);
        }
        */
    }

}