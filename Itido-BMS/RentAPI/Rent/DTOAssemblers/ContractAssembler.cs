using System;
using Rent.DTOs.TimePlanningDTO;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Rent.DTOAssemblers
{
    public class ContractAssembler
    {
        private readonly IAgreementRepository _agreementRepository;
        public ContractAssembler(IAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;

        }

        /*
        
        Functions for DTOs
        
         */

        public System.Linq.Expressions.Expression<Func<Contract, Object>> ContractDTOBasic = contract => new
        {
            ID = contract.ID,

            User =
            contract.User != null ?
            new
            {
                ID = contract.User.ID,
                FirstName = contract.User.FirstName,
                LastName = contract.User.LastName,
                EmployeeNumber = contract.User.EmployeeNumber,
                RoleName = contract.User.Role != null ? contract.User.Role.Name : null
            } : null

        };

        public System.Linq.Expressions.Expression<Func<Contract, Object>> ContractDTODetailed = contract => new
        {
            ID = contract.ID,
            User =
           contract.User != null ?
           new
           {
               ID = contract.User.ID,
               FirstName = contract.User.FirstName,
               LastName = contract.User.LastName,
               EmployeeNumber = contract.User.EmployeeNumber,
               RoleName = contract.User.Role != null ? contract.User.Role.Name : null
           } : null,
            WeeklyHours = contract.WeeklyHours,
            From = contract.From,
            To = contract.To,
            Agreement = contract.Agreement != null ?
           new
           {
               ID = contract.Agreement.ID,
               Name = contract.Agreement.Name

           } : null
        };

        /*

      Functions for DTOs

       */





        public ContractDTO CreateContractDTO(Contract contract)
        {
            return new ContractDTO
            {
                ID = contract.ID,
                AgreementName = contract.Agreement?.Name,
                UserID = contract.UserID,
                AgreementID = contract.AgreementID,
                WeeklyHours = contract.WeeklyHours,
                From = contract.From,
                To = contract.To
            };
        }

        public IEnumerable<Object> CreateContractDTOsFromListOfContracts(IQueryable<Contract> contracts)
        {
            return contracts.ToList().Select(CreateContractDTO);


        }

        public Contract CreateContract(ContractDTO contractDTO, int userId, int agreementId)
        {
            Console.WriteLine("Inside create contract from DTO");
            Console.WriteLine(contractDTO.AgreementName);

            return new Contract
            {
                UserID = userId,
                AgreementID = agreementId,
                WeeklyHours = contractDTO.WeeklyHours,
                From = contractDTO.From,
                To = contractDTO.To,
            };
        }

    }

}