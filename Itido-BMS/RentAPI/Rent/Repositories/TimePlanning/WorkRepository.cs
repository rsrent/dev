using System.Threading.Tasks;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;
using Rent.Models.TimePlanning.Enums;
using Rent.Data;
using System;
using System.Linq;
using Rent.ContextPoint.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using Rent.Helpers;
using Rent.DTOAssemblers;

namespace Rent.Repositories.TimePlanning
{
    public class WorkRepository
    {

        private readonly ProjectRoleRepository _projectRoleRepository;

        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly RentContext _rentContext;

        private readonly NotiRepository _notiRepo;
        private readonly WorkRegistrationRepository _workRegistration;
        private readonly AbsenceHelperRepository _absenceRepository;

        public WorkRepository(ProjectRoleRepository projectRoleRepository, IRoleAuthenticationRepository roleRepo, RentContext rentContext, WorkRegistrationRepository workRegistration, NotiRepository notiRepo, AbsenceHelperRepository absenceRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            _roleRepo = roleRepo;
            _rentContext = rentContext;
            _workRegistration = workRegistration;
            _notiRepo = notiRepo;
            _absenceRepository = absenceRepository;
        }


        private static bool BetweenDates(Work w, DateTime from, DateTime to) => w.Date >= from && w.Date <= to;
        private static bool Invited(Work w, int userId) => w.WorkInvitations.Any(wi => wi.Contract.UserID == userId);
        private static bool WorkLate(Work work)
        {
            if (work.WorkRegistration != null && work.WorkRegistration.Date.Date > work.Date.Date)
            {
                return true;
            }
            if (work.WorkRegistration == null && work.Date < DateTime.Now.AddDays(-1))
            {
                return true;
            }

            return false;
        }

        public Work Get(int requester, int workId)
        {
            var userRole = _roleRepo.GetRole(requester);
            return _projectRoleRepository.GetReadableWorkOfUser(requester)
                .FirstOrDefault(w => w.ID == workId);
        }

        public dynamic GetDTO(int requester, int workId)
        {
            return Work.DetailedDTO(requester, _roleRepo.GetRole(requester)).Compile()(Get(requester, workId));
        }


        // GET ALL WORK
        public IQueryable<Work> GetAllWork(int requester, DateTime? start = null, DateTime? end = null)
        {
            var workOfUserQuery = _projectRoleRepository
                    .GetReadableWorkOfUser(requester);

            if (start != null && end != null)
                return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value));

            start = DateTime.Now.AddDays(-2);
            end = DateTime.Now.AddDays(14);

            return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || WorkLate(w));
        }

        public IQueryable<dynamic> GetAllWorkDTO(int requester, DateTime? start = null, DateTime? end = null)
            => GetAllWork(requester, start, end).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));


        // GET ALL WORK OF USER
        public IQueryable<Work> GetAllWorkOfUser(int requester, int userId, DateTime? start = null, DateTime? end = null)
        {
            var workOfUserQuery = _projectRoleRepository
                    .GetReadableWorkOfUser(requester)
                    .AssociatedWithUser(userId);

            if (start != null && end != null)
                return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || (BetweenDates(w, start.Value, end.Value) && Invited(w, userId)));

            start = DateTime.Now.AddDays(-2);
            end = DateTime.Now.AddDays(14);

            return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || WorkLate(w) || Invited(w, userId));
        }

        public IQueryable<dynamic> GetAllWorkOfUserDTO(int requester, int userId, DateTime? start = null, DateTime? end = null)
            => GetAllWorkOfUser(requester, userId, start, end).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));


        // GET ALL WORK OF SIGNED IN USER
        public IQueryable<Work> GetWorkOfSignedInUser(int requester, DateTime? start = null, DateTime? end = null)
        {
            var workOfUserQuery = _projectRoleRepository
                    .GetReadableWorkOfUser(requester)
                    .OwnedByUser(requester);

            if (start != null && end != null)
                return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || (BetweenDates(w, start.Value, end.Value) && Invited(w, requester)));

            start = DateTime.Now.AddDays(-2);
            end = DateTime.Now.AddDays(14);

            return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || WorkLate(w) || Invited(w, requester));
        }

        public IQueryable<dynamic> GetWorkOfSignedInUserDTO(int requester, DateTime? start = null, DateTime? end = null)
            => GetWorkOfSignedInUser(requester, start, end).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));


        // GET ALL WORK OF PROJECT ITEM
        public IQueryable<Work> GetWorkOfProjectItem(int requester, int projectItemId, DateTime? start = null, DateTime? end = null)
        {
            var workOfUserQuery = _projectRoleRepository
                    .GetReadableWorkOfUser(requester)
                    .Where(w => w.ProjectItemID == projectItemId);

            if (start != null && end != null)
                return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value));

            start = DateTime.Now.AddDays(-2);
            end = DateTime.Now.AddDays(14);

            return workOfUserQuery
                    .Where(w =>
                    BetweenDates(w, start.Value, end.Value)
                    || WorkLate(w));
        }

        public IQueryable<dynamic> GetWorkOfProjectItemDTO(int requester, int projectItemId, DateTime? start = null, DateTime? end = null)
            => GetWorkOfProjectItem(requester, projectItemId, start, end).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));

        // GET ALL FREE WORK
        public IQueryable<Work> GetFreeWork(int requester)
        {
            return _projectRoleRepository
                    .GetReadableWorkOfUser(requester)
                    .Where(w => w.Contract == null || (w.WorkReplacement != null && w.WorkReplacement.Contract == null));
        }

        public IQueryable<dynamic> GetFreeWorkDTO(int requester)
            => GetFreeWork(requester).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));


        // GET ALL LATE WORK
        public IQueryable<Work> GetLateWork(int requester)
        {
            return _projectRoleRepository
                    .GetReadableWorkOfUser(requester)
                    .Where(w => WorkLate(w));
        }

        public IQueryable<dynamic> GetLateWorkDTO(int requester)
            => GetLateWork(requester).Select(Work.StandardDTO(requester, _roleRepo.GetRole(requester)));

        /*
        public IQueryable<Object> GetAllWorkForSignedInUserDTO(int requester, DateTime start, DateTime end)
        {
            var requesterRole = _roleRepo.GetRole(requester);



            //Returns All work where the signed in user is either the one on the with the shift or the replacer
            return GetAllWorkDTO(
                w => w.Date >= start && w.Date <= end
                && ((w.ContractID != null
                    && w.Contract.UserID == requester
                    && w.WorkReplacement == null)
                || (w.WorkReplacement != null
                    && w.WorkReplacement.ContractID != null
                    && w.WorkReplacement.Contract.UserID == requester) || LateRegistration(w)), Work.StandardDTO(requester, requesterRole));

        }

        public IQueryable<Object> GetAllWorkForSignedInUserDTOInTwoWeeks(int requester)
        {
            var today = DateTime.Now;
            var twoWeeksFromNow = DateTimeHelpers.AddDays(today, 14);
            return GetAllWorkForSignedInUserDTO(requester, today, twoWeeksFromNow);
        }
        */
        /*
        public IQueryable<Object> GetAllWorkForLocationDTO(int requester, int locationId, DateTime start, DateTime end)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return GetAllWorkDTO(w => w.Date >= start && w.Date <= end && w.LocationID == locationId, Work.BasicDTO(requester, requesterRole));
            }
            throw new UnauthorizedAccessException();
        }


        public IEnumerable<Work> GetAllFreeWorkDTO(int requester)
        {
            throw new NotImplementedException();

        }
        */
        /*
        public Object GetWorkDTO(int requester, int workId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                var shift = _rentContext.Work
                .Include(w => w.Contract)
                .ThenInclude(c => c.User)
                .Include(w => w.WorkReplacement)
                .ThenInclude(wr => wr.Contract)
                .Include(w => w.WorkRegistration)
                .Where(s => s.ID == workId)
                .Select(Work.DetailedDTO(requester, requesterRole))
                .FirstOrDefault();
                if (shift == null)
                {
                    throw new NotFoundException();
                }
                return shift;
            }
            throw new UnauthorizedAccessException();
        }
        */
        /*
        public IQueryable<T> GetAllWorkDTO<T>(System.Linq.Expressions.Expression<Func<Work, bool>> predicate, System.Linq.Expressions.Expression<Func<Work, T>> selector)
        {
            return _rentContext.Work
             //.Include(w => w.Contract)
             //.ThenInclude(c => c.User)
             .Include(w => w.WorkReplacement)
             .ThenInclude(wr => wr.Contract)
             .ThenInclude(c => c.User)
             .Include(w => w.WorkRegistration)
            //.Include(w => w.ProjectItem)
            //.ThenInclude(l => l.Project)
            //.ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent).ThenInclude(p => p.Parent)
            .Where(predicate)
            .Select(selector);
        }


        public IQueryable<object> GetAllWorkInNextTwoWeek(int requester)
        {
            var today = DateTime.Now;
            var twoWeeksFromNow = DateTimeHelpers.AddDays(today, 14);

            return GetAllWorkInPeriodDTO(requester, today, twoWeeksFromNow);

        }

        public IQueryable<object> GetAllWorkInPeriodForWorkContractDTO(int requester, int workContractId, DateTime start, DateTime end)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return GetAllWorkDTO(w => w.Date >= start && w.Date <= end && w.WorkContractID == workContractId, Work.StandardDTO(requester, requesterRole));
            }
            throw new UnauthorizedAccessException();

        }


        public IQueryable<Object> GetAllWorkInPeriodDTO(int requester, DateTime start, DateTime end)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {

                var requesterRole = _roleRepo.GetRole(requester);
                return GetAllWorkDTO(w => w.Date >= start && w.Date <= end, Work.StandardDTO(requester, requesterRole));
            }
            throw new UnauthorizedAccessException();
        } 
        */

        public async Task AddContract(int requester, int workId, int contractId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Work work = _rentContext.Work.Include(w => w.WorkInvitations).FirstOrDefault(w => w.ID == workId);
                if (work != null && work.ContractID == null && _absenceRepository.IsContractAvailableOnDate(contractId, work.Date))
                {
                    work.ContractID = contractId;
                    _rentContext.Work.Update(work);
                    _rentContext.WorkInvitation.RemoveRange(work.WorkInvitations);
                    await _rentContext.SaveChangesAsync();
                    return;
                }
                else
                {
                    throw new NullReferenceException();
                }
            }
            throw new UnauthorizedAccessException();
        }

        public async Task RemoveContract(int requester, int workId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Work work = _rentContext.Work.Include(w => w.WorkReplacement).FirstOrDefault(w => w.ID == workId);
                if (work != null)
                {
                    work.ContractID = null;
                    if (work.WorkReplacement != null)
                    {
                        work.ContractID = work.WorkReplacement.ContractID;
                        _rentContext.WorkReplacement.Remove(work.WorkReplacement);
                    }
                    _rentContext.Work.Update(work);
                    await _rentContext.SaveChangesAsync();
                    return;
                }
                else
                {
                    throw new NullReferenceException();
                }

            }
            throw new UnauthorizedAccessException();

        }

        public async Task AddReplacer(int requester, int workId, int contractId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                Work work = _rentContext.Work.Include(w => w.WorkInvitations).FirstOrDefault(w => w.ID == workId);
                WorkReplacement workReplacement = _rentContext.WorkReplacement.FirstOrDefault(w => w.WorkID == workId);
                if (work != null && workReplacement != null && _absenceRepository.IsContractAvailableOnDate(contractId, work.Date))
                {
                    workReplacement.ContractID = contractId;

                    _rentContext.WorkInvitation.RemoveRange(work.WorkInvitations);
                    _rentContext.WorkReplacement.Update(workReplacement);
                    await _rentContext.SaveChangesAsync();
                    return;
                }
                throw new NullReferenceException();
            }
            throw new UnauthorizedAccessException();
        }

        public async Task RemoveReplacer(int requester, int workId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                WorkReplacement workReplacement = _rentContext.WorkReplacement.FirstOrDefault(w => w.WorkID == workId);
                if (workReplacement != null)
                {
                    workReplacement.ContractID = null;

                    _rentContext.WorkReplacement.Update(workReplacement);
                    await _rentContext.SaveChangesAsync();
                    return;
                }
                throw new NullReferenceException();

            }
            throw new UnauthorizedAccessException();

        }

        public async Task<int> RegisterWork(int requester, int workId, short startTime = -1, short endTime = -1)
        {
            var newWork = _rentContext.Work.Include(w => w.WorkRegistration).FirstOrDefault(w => w.ID == workId);
            if (newWork.WorkRegistration == null)
            {
                int workRegistrationId;
                if (startTime == -1 && endTime == -1)
                {
                    workRegistrationId = await _workRegistration.RegisterWork(requester, workId);
                }
                else
                {
                    workRegistrationId = await _workRegistration.RegisterWorkWithCustomtTime(requester, workId, startTime, endTime);
                }

                newWork.Modifications = FlagsHelper.Set<int>(newWork.Modifications, (int)WorkModificationFlags.Registered);
                _rentContext.Work.Update(newWork);
                await _rentContext.SaveChangesAsync();
                return workRegistrationId;
            }
            throw new UnauthorizedAccessException();
        }

        public async Task ReplyToRegistration(int requester, int regstrationId, bool answer)
        {
            await _workRegistration.ReplyToRegistration(requester, regstrationId, answer);
        }
        public async Task UpdateWork(int requester, Work work)
        {

            if (work == null)
            {
                throw new NothingUpdatedException();
            }

            if (_roleRepo.IsAdminOrManager(requester))
            {
                var oldWork = _rentContext.Work.Find(work.ID);

                if (work.BreakMins != oldWork.BreakMins)
                {
                    oldWork.BreakMins = work.BreakMins;
                    oldWork.Modifications = FlagsHelper.Set<int>(oldWork.Modifications, (int)WorkModificationFlags.BreakMins);
                }

                if (work.StartTimeMins != oldWork.StartTimeMins)
                {
                    oldWork.StartTimeMins = work.StartTimeMins;
                    oldWork.Modifications = FlagsHelper.Set<int>(oldWork.Modifications, (int)WorkModificationFlags.StartTimeMins);
                }

                if (work.EndTimeMins != oldWork.EndTimeMins)
                {
                    oldWork.EndTimeMins = work.EndTimeMins;
                    oldWork.Modifications = FlagsHelper.Set<int>(oldWork.Modifications, (int)WorkModificationFlags.EndTimeMins);
                }
                if (work.IsVisible != oldWork.IsVisible)
                {
                    oldWork.IsVisible = work.IsVisible;
                    oldWork.Modifications = FlagsHelper.Set<int>(oldWork.Modifications, (int)WorkModificationFlags.IsVisible);
                }
                if (work.Note != oldWork.Note)
                {
                    oldWork.Note = work.Note;
                    oldWork.Modifications = FlagsHelper.Set<int>(oldWork.Modifications, (int)WorkModificationFlags.Note);
                }

                _rentContext.Work.Update(oldWork);
                await _rentContext.SaveChangesAsync();
                return;
            }
            throw new UnauthorizedAccessException();
        }



        // PROJECT

        public async Task<int> CreateForProjectItem(int requester, Work work, int projectItemId)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                work.Modifications = 0;
                work.ProjectItemID = projectItemId;
                _rentContext.Work.Add(work);
                await _rentContext.SaveChangesAsync();
                return work.ID;

            }
            throw new UnauthorizedAccessException();
        }
        /*
        public IQueryable<Object> GetOfProjectItem(int requester, int projectItemId, DateTime start, DateTime end)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var requesterRole = _roleRepo.GetRole(requester);
                return GetAllWorkDTO(w => w.Date >= start && w.Date <= end && w.ProjectItemID == projectItemId, Work.DetailedDTO(requester, requesterRole));
            }
            throw new UnauthorizedAccessException();
        }
        */


    }

    public static class WorkQueryExtentions
    {
        public static IQueryable<Work> BetweenDates(this IQueryable<Work> query, DateTime from, DateTime to)
            => query.Where(w => w.Date >= from && w.Date <= to);

        public static IQueryable<Work> AssociatedWithUser(this IQueryable<Work> query, int userId)
            => query.Where(w => (w.ContractID != null && w.Contract.UserID == userId)
            || (w.WorkReplacement != null && w.WorkReplacement.ContractID != null && w.WorkReplacement.Contract.UserID == userId));

        public static IQueryable<Work> OwnedByUser(this IQueryable<Work> query, int userId)
            => query.Where(w => (w.ContractID != null && w.Contract.UserID == userId && w.WorkReplacement == null)
            || (w.WorkReplacement != null && w.WorkReplacement.ContractID != null && w.WorkReplacement.Contract.UserID == userId));

    }
}