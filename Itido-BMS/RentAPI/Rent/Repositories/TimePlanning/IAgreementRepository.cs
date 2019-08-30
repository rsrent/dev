using System.Collections.Generic;
using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using System;
using System.Linq;

namespace Rent.Repositories.TimePlanning
{
    public interface IAgreementRepository
    {
        Task<int> CreateAgreement(int requester, Agreement newAgreement);
        Object Get(int requester, int id);
        IQueryable<Object> GetAll(int requester);
        Task UpdateAgreement(int requester, Agreement agreement);
    }
}