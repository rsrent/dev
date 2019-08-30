using System;
using Rent.Models.TimePlanning;
using Rent.Models;
using Rent.Repositories.TimePlanning;
using System.Collections.Generic;
using Rent.DTOs.TimePlanningDTO;
using Rent.Repositories;

namespace Rent.DTOAssemblers
{
    public class WorkContractListAssembler
    {
        private readonly UserListDTOAssembler _userListDTOAssembler;
        private readonly ContractAssembler _contractAssembler;
        public WorkContractListAssembler(UserListDTOAssembler userListDTOAssembler, ContractAssembler contractAssembler)
        {
            _userListDTOAssembler = userListDTOAssembler;
            _contractAssembler = contractAssembler;
        }


        /**
        
            Functions for DTOs
        
         */



        public System.Linq.Expressions.Expression<Func<WorkContract, Object>> WorkContractBasic = workContract => new
        {
            ID = workContract.ID,
            Note = workContract.Note,
            ToDate = workContract.ToDate,
            FromDate = workContract.FromDate,
            IsVisible = workContract.IsVisible,
            Contract = workContract.Contract != null ? new
            {
                Id = workContract.Contract.ID,
                User = workContract.Contract.User != null ? new
                {
                    ID = workContract.Contract.User.ID,
                    FirstName = workContract.Contract.User.FirstName,
                    LastName = workContract.Contract.User.LastName,
                    EmployeeNumber = workContract.Contract.User.EmployeeNumber,
                    RoleName = workContract.Contract.User.Role != null ? workContract.Contract.User.Role.Name : null

                } : null
            } : null

        };

        public System.Linq.Expressions.Expression<Func<WorkContract, Object>> WorkContractDetailed = workContract => new
        {
            ID = workContract.ID,
            Note = workContract.Note,
            FromDate = workContract.FromDate,
            ToDate = workContract.ToDate,
            IsVisible = workContract.IsVisible,
            Contract = workContract.Contract != null ? new
            {
                ID = workContract.Contract.ID,
                User =
               workContract.Contract.User != null ?
               new
               {
                   ID = workContract.Contract.User.ID,
                   FirstName = workContract.Contract.User.FirstName,
                   LastName = workContract.Contract.User.LastName,
                   EmployeeNumber = workContract.Contract.User.EmployeeNumber,
                   RoleName = workContract.Contract.User.Role != null ? workContract.Contract.User.Role.Name : null
               } : null,
                WeeklyHours = workContract.Contract.WeeklyHours,
                From = workContract.Contract.From,
                To = workContract.Contract.To,
                Agreement = workContract.Contract.Agreement != null ?
           new
           {
               ID = workContract.Contract.Agreement.ID,
               Name = workContract.Contract.Agreement.Name

           } : null
            } : null,
            WorkDays = workContract.WorkDays != null ? workContract.WorkDays : null,
            WorkHoliday = workContract.WorkHolidays != null ? workContract.WorkHolidays : null

        };



        /**
        
            Functions for DTOs
        
         */


        public WorkContractListDTO CreateWorkContractListDTO(WorkContract workContract)
        {
            UserListDTO userListDTO = (workContract.Contract?.User) != null ? _userListDTOAssembler.CreateUserListDTO(workContract.Contract.User) : null;

            return new WorkContractListDTO
            {
                ID = workContract.ID,
                User = userListDTO,
                Note = workContract.Note,

                FromDate = workContract.FromDate,
                ToDate = workContract.ToDate
            };

        }

        public WorkContractListDTO CreateWorkContractListDTODetailed(WorkContract workContract)
        {
            UserListDTO userListDTO = _userListDTOAssembler.CreateUserListDTO(workContract.Contract.User);

            return new WorkContractListDTO
            {
                ID = workContract.ID,
                User = userListDTO,
                Contract = workContract.Contract,
                WorkDays = workContract.WorkDays,
                Holidays = workContract.WorkHolidays,
                Note = workContract.Note,

                FromDate = workContract.FromDate,
                ToDate = workContract.ToDate
            };

        }


        public IEnumerable<WorkContractListDTO> CreateWorkContractListDTOList(IEnumerable<WorkContract> workContracts)
        {
            var workContractListDTOs = new List<WorkContractListDTO>();
            foreach (var contract in workContracts)
            {
                Console.WriteLine("Inside create work contract dtos loop");
                workContractListDTOs.Add(CreateWorkContractListDTO(contract));
            }
            return workContractListDTOs;
        }


    }
}