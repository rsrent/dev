using System;
using System.Linq.Expressions;

namespace Rent.Models.TimePlanning
{
    public class Request
    {
        public int ID { get; set; }
        public ApprovalState ApprovalState { get; set; }
        public DateTime? RespondDateTime { get; set; }

        public int? CreatorID { get; set; }
        public virtual User Creator { get; set; }

        public int? SubjectID { get; set; }
        public virtual User Subject { get; set; }

        public int? ResponderID { get; set; }
        public virtual User Responder { get; set; }

        public static Expression<Func<Request, User, dynamic>> BasicDTO(int requester, string requesterRole)
        {
            var isRequesterAdminOrManager = requesterRole.Equals("Admin") || requesterRole.Equals("Manager");

            return (request, creator) => request != null ?
            new
            {
                request.ID,
                request.ApprovalState,
                //CanRespondToApprovalState = false,
                CanRespondToApprovalState = request.ApprovalState == ApprovalState.Pending &&
                        (((creator.UserRole.Equals("Admin") || creator.UserRole.Equals("Manager")) && (request.SubjectID == requester) ||
                        (isRequesterAdminOrManager && request.SubjectID == request.CreatorID))),
            } : null;
        }
    }
}
