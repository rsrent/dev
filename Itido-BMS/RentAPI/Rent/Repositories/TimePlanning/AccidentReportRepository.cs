using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models.TimePlanning;

namespace Rent.Repositories.TimePlanning
{

    public class AccidentReportRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;
        private readonly IRequestRepository _requestRepository;
        private readonly NotiRepository _notiRepository;

        public AccidentReportRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo, IRequestRepository approvalStateRepository, NotiRepository notiRepository)
        {
            _rentContext = rentContext;
            _roleRepo = roleRepo;
            _requestRepository = approvalStateRepository;
            _notiRepository = notiRepository;
        }

        public async Task<int> Create(int requester, AccidentReport accidentReport, int userId)
        {
            if (_roleRepo.IsAdminOrManager(requester) || (_roleRepo.IsUser(requester) && accidentReport.UserID == requester))
            {
                var requestId = await _requestRepository.CreateRequest(requester, userId);

                accidentReport.RequestID = requestId;
                accidentReport.UserID = userId;
                accidentReport.CreatorID = requester;

                _rentContext.Add(accidentReport);
                await _rentContext.SaveChangesAsync();

                await _notiRepository.SendAccidentReportRequestNoti(requester, accidentReport);

                return accidentReport.ID;
            }
            throw new UnauthorizedAccessException();
        }

        public dynamic Get(int requester, int accidentReportId)
        {
            var isRequesterAdminOrManager = _roleRepo.IsAdminOrManager(requester);

            var accidentReport = _rentContext.AccidentReports.Include(ar => ar.Request).FirstOrDefault(a => a.ID == accidentReportId);

            if (accidentReport == null)
            {
                throw new NotFoundException();
            }

            if (isRequesterAdminOrManager || (_roleRepo.IsUser(requester) && accidentReport.UserID == requester))
            {
                var selector = DTO(requester, isRequesterAdminOrManager);
                return selector.Compile().Invoke(accidentReport); ;
            }

            throw new UnauthorizedAccessException();
        }

        public IQueryable<dynamic> GetAllOfUser(int requester, int userId)
        {
            var isRequesterAdminOrManager = _roleRepo.IsAdminOrManager(requester);
            if (isRequesterAdminOrManager || (_roleRepo.IsUser(requester) && userId == requester))
            {
                var selector = DTO(requester, isRequesterAdminOrManager);
                return _rentContext.AccidentReports.Include(a => a.Request).Where(a => a.UserID == userId).Select(selector);
            }
            throw new UnauthorizedAccessException();
        }

        public async Task ReplyToAccidentReport(int requester, int accidentReportId, bool isApproved)
        {
            AccidentReport accidentReport = _rentContext.AccidentReports.FirstOrDefault(a => a.ID == accidentReportId);
            if (accidentReport == null)
            {
                throw new NotFoundException();
            }

            var success = await _requestRepository.ReplyToRequest(requester, accidentReport.RequestID, isApproved);
            if (success)
            {
                //_rentContext.AccidentReports.Update(accidentReport);
                //await _rentContext.SaveChangesAsync();

                if (isApproved)
                    await _notiRepository.SendAccidentReportApprovedNoti(requester, accidentReport);
                else
                    await _notiRepository.SendAccidentReportDeclinedNoti(requester, accidentReport);
            }
            else
            {
                throw new UnauthorizedAccessException();
            }
        }

        public System.Linq.Expressions.Expression<Func<AccidentReport, dynamic>> DTO(int requester, bool isRequesterAdminOrManager)
        {
            return accidentReport => new
            {
                ID = accidentReport.ID,
                accidentReport.AccidentReportType,
                accidentReport.AbsenceDurationDays,
                accidentReport.ActionTaken,
                accidentReport.DateTime,
                accidentReport.Description,
                accidentReport.Place,

                Request = accidentReport.Request != null ? new
                {
                    accidentReport.Request.ApprovalState,
                    CanRespondToApprovalState = accidentReport.Request.ApprovalState == ApprovalState.Pending &&
                        (((accidentReport.Request.Creator.UserRole.Equals("Admin") || accidentReport.Request.Creator.UserRole.Equals("Manager")) && accidentReport.Request.SubjectID == requester) ||
                        (isRequesterAdminOrManager && accidentReport.Request.SubjectID == accidentReport.Request.CreatorID)),

                } : null
            };
        }
    }
}
