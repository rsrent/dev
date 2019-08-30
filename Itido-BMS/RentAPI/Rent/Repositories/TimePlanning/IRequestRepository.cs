using System.Threading.Tasks;
using Rent.Models.TimePlanning;

namespace Rent.Repositories.TimePlanning
{
    public interface IRequestRepository
    {
        bool CanRequesterReplyToRequest(int requester, int requestID);
        bool CanRequesterReplyToRequest(int requester, Request request);
        bool CanRequesterReplyToRequest(int requester, ApprovalState approvalState, int creatorId, int subjectId);

        Task<int> CreateRequest(int creatorId, int subjectId);
        Task<bool> ReplyToRequest(int requester, int requestID, bool isApproved);
    }
}