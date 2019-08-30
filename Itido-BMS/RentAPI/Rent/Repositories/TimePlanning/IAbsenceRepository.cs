using System.Collections.Generic;
using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using System;
using System.Linq;

namespace Rent.Repositories.TimePlanning
{
    public interface IAbsenceRepository
    {
        Task<int> CreateAbsence(int requester, Absence absence, int userId, int absenceReadonId, bool isRequest);
        Object GetDTO(int requester, int absenceId);
        IQueryable<Object> GetAllAbsenceOfUserDTO(int requester, int userId);
        //ApprovalState SetInitialStateOfAbsence(int requester, Absence absence, bool isRequest);
        Task ReplyToAbsence(int requester, int absenceId, bool answer);

        Task UpdateAbsence(int requester, Absence absence);
    }
}