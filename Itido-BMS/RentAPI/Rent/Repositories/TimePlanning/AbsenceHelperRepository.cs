using System;
using System.Linq;
using Rent.Data;
using Rent.Models.TimePlanning;

namespace Rent.Repositories.TimePlanning
{
    public class AbsenceHelperRepository
    {
        private readonly RentContext _rentContext;
        public AbsenceHelperRepository(RentContext rentContext)
        {
            _rentContext = rentContext;
        }

        public Absence ContractAbsenceOnDate(int contractId, DateTime date)
        {
            return _rentContext.Absence
                .FirstOrDefault(a => a.User.Contracts.Any(c => c.ID == contractId)
                && a.From <= date && date <= a.To
                && (!a.IsRequest || a.Request.ApprovalState == ApprovalState.Approved));
        }

        public bool IsUserAvailableOnDate(int userId, DateTime date)
        {
            return !_rentContext.Absence
                .Any(a => a.UserID == userId
                && a.From <= date && date <= a.To
                && (!a.IsRequest || a.Request.ApprovalState == ApprovalState.Approved));
        }

        public bool IsContractAvailableOnDate(int contractId, DateTime date)
        {
            return !_rentContext.Absence
                .Any(a => a.User.Contracts.Any(c => c.ID == contractId)
                && a.From <= date && date <= a.To
                && (!a.IsRequest || a.Request.ApprovalState == ApprovalState.Approved));
        }
    }
}
