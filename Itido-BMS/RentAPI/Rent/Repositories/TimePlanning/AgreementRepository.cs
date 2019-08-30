using System;
using Rent.Data;
using Rent.ContextPoint.Exceptions;
using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using System.Linq;

namespace Rent.Repositories.TimePlanning
{
    public class AgreementRepository : IAgreementRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;


        public AgreementRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
        }

        public Object Get(int requester, int id)
        {
            var requesterRole = _roleRepo.GetRole(requester);
            Object agreement = _rentContext.Agreement.Where(a => a.ID == id).Select(Agreement.BasicDTO()).FirstOrDefault();
            if (agreement == null)
            {
                throw new NotFoundException();
            }
            return agreement;

        }

        public IQueryable<Object> GetAll(int requester)
        {
            var requesterRole = _roleRepo.GetRole(requester);
            if (_roleRepo.IsAdmin(requester))
            {
                return _rentContext.Agreement.Select(Agreement.BasicDTO());
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task<int> CreateAgreement(int requester, Agreement newAgreement)
        {

            if (_roleRepo.IsAdmin(requester))
            {
                var agreement = new Agreement
                {
                    Name = newAgreement.Name,
                };
                _rentContext.Add(agreement);
                await _rentContext.SaveChangesAsync();
                return agreement.ID;
            }
            else
            {
                throw new UnauthorizedAccessException();
            }

        }

        public async Task UpdateAgreement(int requester, Agreement agreement)
        {
            if (agreement == null)
            {
                throw new NothingUpdatedException();
            }
            _rentContext.Agreement.Update(agreement);
            await _rentContext.SaveChangesAsync();
            //return agreement.ID;

        }

    }
}