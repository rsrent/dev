
using System;
using System.Threading.Tasks;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Helpers;
using Rent.Models.TimePlanning;

namespace Rent.Repositories.TimePlanning
{
    public class RequestRepository : IRequestRepository
    {
        private readonly RentContext _rentContext;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public RequestRepository(RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _roleRepo = roleRepo;
            _rentContext = rentContext;
        }

        public async Task<int> CreateRequest(int creatorId, int subjectId)
        {
            var request = new Request
            {
                ApprovalState = ApprovalState.Pending,
                CreatorID = creatorId,
                SubjectID = subjectId,
            };
            _rentContext.Requests.Add(request);
            await _rentContext.SaveChangesAsync();
            return request.ID;
        }

        public async Task<bool> ReplyToRequest(int requester, int requestID, bool isApproved)
        {
            var request = _rentContext.Requests.Find(requestID);
            if (request != null)
            {
                var canAnswer = CanRequesterReplyToRequest(requester, request.ApprovalState, request.CreatorID.Value, request.SubjectID.Value);

                if (canAnswer)
                {
                    request.ApprovalState = isApproved ? ApprovalState.Approved : ApprovalState.Denied;
                    request.ResponderID = requester;
                    request.RespondDateTime = DateTimeHelpers.GmtPlusOneDateTime();
                    _rentContext.Requests.Update(request);
                    await _rentContext.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                throw new NotFoundException();
            }
        }

        public bool CanRequesterReplyToRequest(int requester, int requestID)
        {
            var request = _rentContext.Requests.Find(requestID);
            if (request != null)
            {
                return CanRequesterReplyToRequest(requester, request.ApprovalState, request.CreatorID.Value, request.SubjectID.Value);
            }
            else
            {
                return false;
            }
        }

        public bool CanRequesterReplyToRequest(int requester, Request request)
        {
            return CanRequesterReplyToRequest(requester, request.ApprovalState, request.CreatorID.Value, request.SubjectID.Value);
        }

        public bool CanRequesterReplyToRequest(int requester, ApprovalState approvalState, int creatorId, int subjectId)
        {
            if (approvalState == ApprovalState.Pending)
            {
                if (_roleRepo.IsAdminOrManager(creatorId) && requester == subjectId)
                {
                    return true;

                }
                else if (subjectId == creatorId && _roleRepo.IsAdminOrManager(requester))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}