using Rent.Data;
using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using System.Linq;
using System.Collections.Generic;
using Rent.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;

namespace Rent.Repositories.TimePlanning

{

    public class AbsenceReasonRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public AbsenceReasonRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
        }
        public async Task<int> CreateAbsenceReason(int requester, AbsenceReason absenceReason)
        {
            Console.WriteLine("Called create");
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Console.WriteLine("inside if of create");

                _rentContext.AbsenceReason.Add(absenceReason);
                await _rentContext.SaveChangesAsync();
                return absenceReason.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public AbsenceReason Get(int requester, int absenceReasonId)
        {
            AbsenceReason absenceReason = _rentContext.AbsenceReason.Find(absenceReasonId);
            if (absenceReason == null)
            {
                throw new NotFoundException();
            }
            return absenceReason;
        }

        public IEnumerable<AbsenceReason> GetAll(int requester)
        {
            var userIsAdmin = _roleRepo.IsAdmin(requester);
            var userIsManager = _roleRepo.IsManager(requester);
            var userIsUser = _roleRepo.IsUser(requester);

            return _rentContext.AbsenceReason.Where(ar => userIsAdmin || (userIsUser && (ar.CanUserCreate || ar.CanUserRequest)) || (userIsManager && (ar.CanManagerCreate || ar.CanManagerRequest))).ToList();

        }

        public async Task UpdateAbsenceReason(int requester, AbsenceReason absenceReason)
        {
            _rentContext.AbsenceReason.Update(absenceReason);
            await _rentContext.SaveChangesAsync();

        }


    }
}