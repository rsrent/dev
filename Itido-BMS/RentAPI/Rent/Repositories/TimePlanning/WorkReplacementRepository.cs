using System;
using System.Threading.Tasks;
using Rent.Data;
using Rent.Models.TimePlanning;


namespace Rent.Repositories.TimePlanning
{
    public class WorkReplacementRepository
    {
        
        IRoleAuthenticationRepository _roleRepo;

        RentContext _rentContext;

        public WorkReplacementRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
        }
        public async Task<int> CreateWorkReplacement(int requester, WorkReplacement workReplacement)
        {
            if(workReplacement == null)
            {
                throw new NullReferenceException();
            }
            if(_roleRepo.IsAdminOrManager(requester))
            {
                _rentContext.WorkReplacement.Add(workReplacement);
                await _rentContext.SaveChangesAsync();
                //return workReplacement.ID;
            }
            throw new UnauthorizedAccessException();
        }
        

    }

}