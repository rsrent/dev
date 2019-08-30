using Rent.Data;
using Rent.Models.TimePlanning;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Rent.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using Rent.DTOAssemblers;
using Rent.Helpers;


namespace Rent.Repositories.TimePlanning
{
    public class AbsenceRepository : IAbsenceRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly IRequestRepository _requestRepository;
        private readonly NotiRepository _notiRepository;
        private readonly WorkRepository _workRepository;

        public AbsenceRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo, IRequestRepository approvalStateRepository, NotiRepository notiRepository, WorkRepository workRepository)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
            _requestRepository = approvalStateRepository;
            _notiRepository = notiRepository;
            _workRepository = workRepository;
        }
        //Should not be able to create overlapping absence! 
        public async Task<int> CreateAbsence(int requester, Absence absence, int userId, int absenceReadonId, bool isRequest)
        {
            Console.WriteLine("Called Create absence");
            if (_roleRepo.IsAdminOrManager(requester) || (_roleRepo.IsUser(requester) && absence.UserID == requester))
            {
                Console.WriteLine("Got here");

                AbsenceReason absenceReason = _rentContext.AbsenceReason.Find(absenceReadonId);
                var user = _rentContext.User.Find(userId);


                if (absenceReason == null || user == null)
                {
                    throw new NotFoundException();
                }
                if (user.ClientID != null) throw new UnauthorizedAccessException();
                /* 
                if (isRequest && !((_roleRepo.IsAdminOrManager(requester) && absenceReason.CanManagerRequest) || (_roleRepo.IsUser(requester) && absenceReason.CanUserRequest)))
                {
                    throw new UnauthorizedAccessException();
                }
                if (!isRequest && !((_roleRepo.IsAdminOrManager(requester) && absenceReason.CanManagerCreate) || (_roleRepo.IsUser(requester) && absenceReason.CanUserCreate)))
                {
                    throw new UnauthorizedAccessException();
                }*/

                //Check if absence overlaps with other absence
                var absenceStart = absence.From;
                var absenceEnd = absence.To;

                var absencesForUser = _rentContext.Absence.Where(a => a.UserID == userId && absenceEnd > a.From && a.To > absenceStart).ToList();
                if (absencesForUser.Count > 0) throw new NothingUpdatedException();

                var requestId = await _requestRepository.CreateRequest(requester, userId);
                absence.RequestID = requestId;

                absence.UserID = userId;
                absence.AbsenceReasonID = absenceReadonId;

                absence.IsRequest = isRequest;

                absence.CreatorID = requester;
                _rentContext.Add(absence);
                await _rentContext.SaveChangesAsync();

                if (!absence.IsRequest)
                {
                    Console.WriteLine("Call absence takes effect now");
                    await AbsenceTakesEffect(absence);
                }

                if (absence.IsRequest)
                    await _notiRepository.SendAbsenceRequestNoti(requester, absence);
                else
                    await _notiRepository.SendAbsenceCreateNoti(requester, absence);

                return absence.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public Object GetDTO(int requester, int absenceId)
        {
            var requesterRole = _roleRepo.GetRole(requester);
            var isRequesterAdminOrManager = _roleRepo.IsAdminOrManager(requester);
            // Absence absence = (Absence)_rentContext.Absence.Include(a => a.AbsenceReason).Include(a => a.User).Include(a => a.Creator).Where(a => a.ID == absenceId).Select(_absenceAssembler.AbsenceDTO(requester, isRequesterAdminOrManager)).FirstOrDefault();
            var absence = _rentContext.Absence.Include(a => a.AbsenceReason).Include(a => a.User).Include(a => a.Creator).Where(a => a.ID == absenceId).Select(Absence.StandardDTO(requester, requesterRole)).FirstOrDefault();

            if (absence == null)
            {
                throw new NotFoundException();
            }

            if (isRequesterAdminOrManager || (_roleRepo.IsUser(requester) && absence.UserID == requester))
            {
                return absence;
            }

            throw new UnauthorizedAccessException();
        }

        public IQueryable<Object> GetAllAbsenceOfUserDTO(int requester, int userId)
        {
            //Console.WriteLine("Called get all absence of user");
            var isRequesterAdminOrManager = _roleRepo.IsAdminOrManager(requester);
            if (isRequesterAdminOrManager || (_roleRepo.IsUser(requester) && userId == requester))
            {
                //Console.WriteLine("I am inside the Get all absence of user thingy");
                var requesterRole = _roleRepo.GetRole(requester);
                return _rentContext.Absence.Include(a => a.AbsenceReason).Include(a => a.Request).ThenInclude(r => r.Creator).Include(a => a.Request).ThenInclude(r => r.Subject).Include(a => a.User).Where(a => a.UserID == userId)
               .Select(Absence.BasicDTO(requester, requesterRole));
                //.Select(_absenceAssembler.AbsenceDTO(requester, isRequesterAdminOrManager));
            }
            throw new UnauthorizedAccessException();
        }

        public async Task ReplyToAbsence(int requester, int absenceId, bool isApproved)
        {
            Absence absence = _rentContext.Absence.FirstOrDefault(a => a.ID == absenceId);
            if (absence == null)
            {
                throw new NotFoundException();
            }

            var success = await _requestRepository.ReplyToRequest(requester, absence.RequestID, isApproved);
            if (success)
            {
                var wasRequest = absence.IsRequest;

                absence.IsRequest = false;
                _rentContext.Absence.Update(absence);
                await _rentContext.SaveChangesAsync();

                if (absence.IsRequest && isApproved)
                {
                    await AbsenceTakesEffect(absence);
                }

                if (isApproved)
                    await _notiRepository.SendAbsenceApprovedNoti(requester, absence);
                else
                    await _notiRepository.SendAbsenceDeclinedNoti(requester, absence);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task UpdateAbsence(int requester, Absence absence)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                var absenceToUpdate = _rentContext.Absence.Include(a => a.Request).FirstOrDefault(a => a.ID == absence.ID);

                if (absenceToUpdate == null) throw new NotFoundException();

                var absencesForUser = _rentContext.Absence.Where(a => a.UserID == absenceToUpdate.UserID && a.ID != absenceToUpdate.ID && absenceToUpdate.To > a.From && a.To > absenceToUpdate.From).ToList();
                if (absencesForUser.Count > 0) throw new NothingUpdatedException();

                absenceToUpdate.Comment = absence.Comment;
                absenceToUpdate.From = absence.From;
                absenceToUpdate.To = absence.To;

                _rentContext.Absence.Update(absenceToUpdate);
                await _rentContext.SaveChangesAsync();

                if (!absenceToUpdate.IsRequest || (absenceToUpdate.IsRequest && absenceToUpdate.Request.ApprovalState == ApprovalState.Approved))
                {
                    await AbsenceTakesEffect(absenceToUpdate);
                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public async Task AbsenceTakesEffect(Absence absence)
        {
            var contracts = _rentContext.Contract.Include(c => c.WorkReplacements)
                .ThenInclude(wr => wr.Work).Where(c => c.UserID == absence.UserID).ToList();

            var wrToRemoveUserFrom = _rentContext.WorkReplacement
                .Where(wr => wr.Contract != null && wr.Contract.UserID == absence.UserID
                && DateTimeHelpers.DoesSingleDateOverlapWithDates(wr.Work.Date, absence.From, absence.To)).ToList();
            wrToRemoveUserFrom.ForEach(wr => wr.ContractID = null);
            _rentContext.WorkReplacement.UpdateRange(wrToRemoveUserFrom);


            var workWhichWasReplaced = _rentContext.Work.Include(w => w.WorkReplacement).Where(wr => wr.WorkReplacement != null && wr.WorkReplacement.AbsenceID == absence.ID);

            var workWhichNowMustBeReplaced = _workRepository.GetAllWorkOfUser(0, absence.UserID, absence.From, absence.To).Where(w => w.WorkRegistration == null).ToList();

            // oldWork - newWork
            var workReplacementsNoLongerInAbsencePeriod = workWhichWasReplaced.Where(ow => !workWhichNowMustBeReplaced.Any(nw => nw.Date.Equals(ow.Date))).Select(ow => ow.WorkReplacement);

            // newWork - oldWork  ( a ( b ) c )
            var workReplacementsToAdd = workWhichNowMustBeReplaced.Where(nw => !workWhichWasReplaced.Any(ow => nw.Date.Equals(ow.Date))).Select((w) => new WorkReplacement()
            {
                WorkID = w.ID,
                ContractID = null,
                AbsenceID = absence.ID,
            });

            _rentContext.WorkReplacement.RemoveRange(workReplacementsNoLongerInAbsencePeriod);

            _rentContext.WorkReplacement.AddRange(workReplacementsToAdd);

            await _rentContext.SaveChangesAsync();
        }


    }
}