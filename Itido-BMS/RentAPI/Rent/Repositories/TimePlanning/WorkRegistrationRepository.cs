
using System;
using System.Threading.Tasks;
using Rent.Data;
using Rent.Models.TimePlanning;
using Rent.Models.TimePlanning.Enums;
using Microsoft.EntityFrameworkCore;

using System.Linq;
using Rent.ContextPoint.Exceptions;

namespace Rent.Repositories.TimePlanning
{
    public class WorkRegistrationRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly RequestRepository _requestRepo;
        private readonly NotiRepository _notiRepo;
        public WorkRegistrationRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo, RequestRepository requestRepo, NotiRepository notiRepo)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
            _requestRepo = requestRepo;
            _notiRepo = notiRepo;
        }

        //requester = admin/manager: create request
        //new time period: create request
        //Registered to late: create request
        // requester = user: no request 
        public async Task<int> RegisterWork(int requester, int workId)
        {
            var dateNow = DateTime.Now;
            var work = _rentContext.Work.Include(w => w.Contract).FirstOrDefault(w => w.ID == workId);
            WorkRegistration workRegistration = new WorkRegistration
            {
                WorkID = workId,
                Date = dateNow,
                UserID = requester,
                StartTimeMins = work.StartTimeMins,
                EndTimeMins = work.EndTimeMins,
                BreakMins = work.BreakMins
            };
            _rentContext.WorkRegistration.Add(workRegistration);
            await _rentContext.SaveChangesAsync();

            return workRegistration.ID;
        }

        public async Task<int> RegisterWorkWithCustomtTime(int requester, int workId, short startMins, short endMins)
        {
            var work = _rentContext.Work.Include(w => w.Contract).FirstOrDefault(w => w.ID == workId);
            int? requestId = null;

            if (work.Contract?.UserID != null)
            {
                requestId = await _requestRepo.CreateRequest(requester, work.Contract.UserID);
            }

            WorkRegistration workRegistration = new WorkRegistration
            {
                WorkID = workId,
                Date = DateTime.Now,
                UserID = requester,
                StartTimeMins = startMins,
                EndTimeMins = endMins,
                BreakMins = work.BreakMins,
                RequestID = requestId,

            };
            _rentContext.WorkRegistration.Add(workRegistration);
            await _rentContext.SaveChangesAsync();
            return workRegistration.ID;
        }

        public async Task ReplyToRegistration(int requester, int workRegistrationId, bool isApproved)
        {
            var workRegistration = _rentContext.WorkRegistration.FirstOrDefault(wr => wr.ID == workRegistrationId);
            if (!workRegistration.RequestID.HasValue) throw new NothingUpdatedException();

            var succes = await _requestRepo.ReplyToRequest(requester, workRegistration.RequestID.Value, isApproved);
            if (succes)
            {
                var request = _rentContext.Requests.FirstOrDefault(r => r.ID == workRegistration.RequestID.Value);
                if (request.ApprovalState == ApprovalState.Approved)
                {
                    await _notiRepo.SendRegistationApprovedNoti(requester, workRegistration);
                }
                else
                {
                    await _notiRepo.SendRegistationDeclinedNoti(requester, workRegistration);

                }
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

    }
}

