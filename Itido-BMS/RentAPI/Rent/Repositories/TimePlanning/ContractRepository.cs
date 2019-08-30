using System;
using Rent.Data;
using Rent.Models.TimePlanning;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Rent.DTOAssemblers;
using Rent.ContextPoint.Exceptions;

namespace Rent.Repositories.TimePlanning
{
    public class ContractRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public ContractRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;

        }

        public dynamic Get(int requester, int contractId)
        {
            return _rentContext.Contract.Include(c => c.Agreement).Include(c => c.User).Where(c => c.ID == contractId).Select(Contract.DetailedDTO()).FirstOrDefault();
        }

        public IQueryable<dynamic> GetAllContractsForUser(int requester, int userId)
        {
            Console.WriteLine(userId);
            return _rentContext.Contract.Include(c => c.User).Where(c => c.UserID == userId).Select(Contract.DetailedDTO());
        }

        public async Task UpdateContract(int requester, Contract contract)
        {
            if (_roleRepo.IsAdmin(requester))
            {
                var contractToUpdate = _rentContext.Contract.Find(contract.ID);

                if (contractToUpdate == null)
                {
                    throw new NotFoundException();
                }

                contractToUpdate.WeeklyHours = contract.WeeklyHours;
                contractToUpdate.From = contract.From;
                contractToUpdate.To = contract.To;

                _rentContext.Contract.Update(contract);
                await _rentContext.SaveChangesAsync();

            }
            throw new UnauthorizedAccessException();
        }

        public async Task CreateContract(int requester, Contract newContract)
        {
            if (_roleRepo.IsAdmin(requester))
            {
                Console.WriteLine("Inside of create contract in repo if statement");
                var user = _rentContext.User.Find(newContract.UserID);

                if (user == null || user.ClientID != null) throw new UnauthorizedAccessException();

                _rentContext.Contract.Add(newContract);
                await _rentContext.SaveChangesAsync();
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}