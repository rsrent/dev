using System;
using System.Threading.Tasks;
using Rent.Data;
using Rent.Models.TimePlanning;
using System.Linq;
using System.Collections.Generic;
using Rent.Models;
using Rent.ContextPoint.Exceptions;
using Rent.Repositories.Language;

namespace Rent.Repositories.TimePlanning
{
    public class NotiRepository
    {
        private readonly RentContext _context;
        private readonly FirebaseNotificationRepository _firebaseNotificationRepository;

        public NotiRepository(RentContext context, FirebaseNotificationRepository firebaseNotificationRepository)
        {
            this._context = context;
            this._firebaseNotificationRepository = firebaseNotificationRepository;
        }

        User GetSender(int requester) => _context.User.Find(requester);

        Translations GetUserTranslationer(int userId) => Translations.GetTranslations(_context.User.Find(userId).LanguageCode);

        NotiReceiver GetNotiReceiver(int requester, int subjectId) => (requester.Equals(subjectId) ?
                        (NotiReceiver)new UsersManagersAndAdminsReceiver { UserId = subjectId } :
                        (NotiReceiver)new UserReceiver { UserId = subjectId });

        public async Task SendAbsenceCreateNoti(int requester, Absence absence)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, absence.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = absence.UserID;
                noti.NotiType = Noti.AbsenceCreated;
                noti.Title = translations.NotiTitleAbsenceCreated();
                noti.Body = translations.NotiBodyAbsenceCreated(sender);
            });
        }

        public async Task SendAbsenceRequestNoti(int requester, Absence absence)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, absence.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = absence.UserID;
                noti.NotiType = Noti.AbsenceRequested;
                noti.Title = translations.NotiTitleAbsenceRequested();
                noti.Body = translations.NotiBodyAbsenceRequested(sender);
            });
        }

        public async Task SendAbsenceApprovedNoti(int requester, Absence absence)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, absence.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = absence.UserID;
                noti.NotiType = Noti.AbsenceApproved;
                noti.Title = translations.NotiTitleAbsenceApproved();
                noti.Body = translations.NotiBodyAbsenceApproved(sender);
            });
        }

        public async Task SendAbsenceDeclinedNoti(int requester, Absence absence)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, absence.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = absence.UserID;
                noti.NotiType = Noti.AbsenceDeclined;
                noti.Title = translations.NotiTitleAbsenceDeclined();
                noti.Body = translations.NotiBodyAbsenceDeclined(sender);
            });
        }

        public async Task SendRegistationApprovedNoti(int requester, WorkRegistration workRegistration)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, workRegistration.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = workRegistration.UserID;
                noti.NotiType = Noti.WorkRegistrationApproved;
                noti.Title = translations.NotiTitleRegistrationApproved(sender, workRegistration);
                noti.Body = translations.NotiBodyRegistrationApproved(sender, workRegistration);
            });
        }

        public async Task SendRegistationDeclinedNoti(int requester, WorkRegistration workRegistration)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, workRegistration.UserID);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.UserID = workRegistration.UserID;
                noti.NotiType = Noti.WorkRegistrationApproved;
                noti.Title = translations.NotiTitleRegistrationDeclined(sender, workRegistration);
                noti.Body = translations.NotiBodyRegistrationDeclined(sender, workRegistration);
            });
        }
        /*
        public async Task SendPostNoti(int requester, Post post)
        {
            var receivers = _context.User.Where(u =>
                post.UserRole != null && u.UserRole.Equals(post.UserRole) ||
                post.LocationID != null && u.LocationUsers.Any(lu => lu.LocationID == post.LocationID) ||
                post.CustomerID != null && u.LocationUsers.Any(lu => lu.Location.Customer.ID.Equals(post.CustomerID)))
                .Select(u => u.ID).ToList();

            Console.WriteLine("Finding receivers...");
            receivers.ForEach((obj) => Console.WriteLine("Receiver: " + obj));

            NotiReceiver notiReceiver = new Users(receivers);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = post.ID;
                noti.NotiType = Noti.Post;
                noti.Title = translations.NotiTitlePostReceived(sender);
                noti.Body = translations.NotiBodyPostReceived(sender, post);
            });
        }
        */

        public async Task SendAccidentReportRequestNoti(int requester, AccidentReport accidentReport)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, accidentReport.UserID.Value);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = accidentReport.ID;
                noti.UserID = accidentReport.UserID.Value;
                noti.NotiType = Noti.AccidentReportRequested;
                noti.Title = translations.NotiTitleAccidentReportRequested(accidentReport);
                noti.Body = translations.NotiBodyAccidentReportRequested(sender, accidentReport);
            });
        }

        public async Task SendAccidentReportApprovedNoti(int requester, AccidentReport accidentReport)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, accidentReport.UserID.Value);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = accidentReport.ID;
                noti.UserID = accidentReport.UserID.Value;
                noti.NotiType = Noti.AccidentReportRequested;
                noti.Title = translations.NotiTitleAccidentReportRequested(accidentReport);
                noti.Body = translations.NotiBodyAccidentReportRequested(sender, accidentReport);
            });
        }

        public async Task SendAccidentReportDeclinedNoti(int requester, AccidentReport accidentReport)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, accidentReport.UserID.Value);
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = accidentReport.ID;
                noti.UserID = accidentReport.UserID.Value;
                noti.NotiType = Noti.AccidentReportDeclined;
                noti.Title = translations.NotiTitleAccidentReportDeclined(accidentReport);
                noti.Body = translations.NotiBodyAccidentReportDeclined(sender, accidentReport);
            });
        }

        public async Task SendUserAddedToWorkContractNoti(int requester, WorkContract workContract)
        {
            NotiReceiver notiReceiver = new Users(new List<int> { workContract.Contract.UserID });
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = workContract.ID;
                noti.UserID = workContract.Contract.UserID;
                noti.NotiType = Noti.WorkContractUserAdded;
                noti.Title = translations.NotiTitleWorkContractUserAdded(sender);
                noti.Body = translations.NotiBodyWorkContractUserAdded(sender, workContract);
            });
        }


        public async Task SendUserRemovedToWorkContractNoti(int requester, WorkContract workContract)
        {
            NotiReceiver notiReceiver = new Users(new List<int> { workContract.Contract.UserID });
            var sender = GetSender(requester);

            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = workContract.ID;
                noti.UserID = workContract.Contract.UserID;
                noti.NotiType = Noti.WorkContractUserAdded;
                noti.Title = translations.NotiTitleWorkContractUserRemoved(sender);
                noti.Body = translations.NotiBodyWorkContractUserRemoved(sender, workContract);
            });
        }

        public async Task SendWorkInvitationNoti(int requester, WorkInvitation invitation)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, invitation.Contract.UserID);
            var sender = GetSender(requester);
            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = invitation.ID;
                noti.UserID = invitation.Contract.UserID;
                noti.NotiType = Noti.InvitationSend;
                noti.Title = translations.NotiTitleInvitation(sender, invitation);
                noti.Body = translations.NotiBodyInvitation(sender, invitation);
            });
        }

        public async Task SendWorkReplacementInviteNoti(int requester, WorkInvitation invitation)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, invitation.Contract.UserID);
            var sender = GetSender(requester);
            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = invitation.ID;
                noti.UserID = invitation.Contract.UserID;
                noti.NotiType = Noti.InvitationSend;
                noti.Title = translations.NotiTitleReplacementInvitation(sender, invitation);
                noti.Body = translations.NotiBodyReplacementInvitation(sender, invitation);
            });
        }

        public async Task SendInvitationAcceptedNoti(int requester, WorkInvitation invitation)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, invitation.Contract.UserID);
            var sender = GetSender(requester);
            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = invitation.ID;
                noti.UserID = invitation.Contract.UserID;
                noti.NotiType = Noti.WorkContractUserAdded;
                noti.Title = translations.NotiTitleInvitationAccept(sender, invitation);
                noti.Body = translations.NotiBodyInvitationAccept(sender, invitation);
            });
        }

        public async Task SendInvitationDeclinedNoti(int requester, WorkInvitation invitation)
        {
            NotiReceiver notiReceiver = GetNotiReceiver(requester, invitation.Contract.UserID);
            var sender = GetSender(requester);
            await SendNotification(notiReceiver, (noti) =>
            {
                var translations = GetUserTranslationer(noti.ReceiverID);

                noti.SenderID = requester;
                noti.SubjectID = invitation.ID;
                noti.UserID = invitation.Contract.UserID;
                noti.NotiType = Noti.WorkContractUserAdded;
                noti.Title = translations.NotiTitleInvitationDecline(sender, invitation);
                noti.Body = translations.NotiBodyInvitationDecline(sender, invitation);
            });
        }





        public async Task SendNotification(NotiReceiver receiver, Action<Noti> prepareNoti)
        {

            List<int> receiverIds = new List<int>();

            if (receiver is UserReceiver)
            {

                receiverIds.Add(((UserReceiver)receiver).UserId);
            }

            if (receiver is UsersManagersAndAdminsReceiver)
            {
                var managerOfThisUserId = ((UsersManagersAndAdminsReceiver)receiver).UserId;
                var _userIds = _context.User.Where(u =>
                u.UserRole.Equals("Admin") ||
                u.UserRole.Equals("Manager") &&
                u.LocationUsers.Any(lu =>
                    lu.Location.LocationUsers.Any(lu1 =>
                        lu1.UserID == managerOfThisUserId))).Select(u => u.ID).ToList();

                receiverIds.AddRange(_userIds);
            }
            if (receiver is Users)
            {
                receiverIds.AddRange(((Users)receiver).UserIds);
            }

            var notis = new List<Noti>();

            foreach (var receiverId in receiverIds)
            {
                var not = new Noti
                {
                    ReceiverID = receiverId,
                    SendTime = DateTime.UtcNow,
                };
                prepareNoti(not);
                notis.Add(not);

            }
            _context.Notis.AddRange(notis);
            await _context.SaveChangesAsync();
            await _firebaseNotificationRepository.PushNotificationFromNotis(notis);
        }
        /*
        public async Task SendNotification(int requester, NotiReceiver receiver, string notiType, int subjectId)
        {

            List<int> receiverIds = new List<int>();

            if (receiver is UserReceiver)
            {

                receiverIds.Add(((UserReceiver)receiver).UserId);
            }

            if (receiver is UsersManagersAndAdminsReceiver)
            {
                var managerOfThisUserId = ((UsersManagersAndAdminsReceiver)receiver).UserId;
                var _userIds = _context.User.Where(u =>
                u.UserRole.Equals("Admin") ||
                u.UserRole.Equals("Manager") &&
                u.LocationUsers.Any(lu =>
                    lu.Location.LocationUsers.Any(lu1 =>
                        lu1.UserID == managerOfThisUserId))).Select(u => u.ID).ToList();

                receiverIds.AddRange(_userIds);
            }

            foreach (var receiverId in receiverIds)
            {
                var not = new Noti
                {
                    SenderID = requester,
                    ReceiverID = receiverId,
                    SendTime = DateTime.UtcNow,
                    NotiType = notiType,
                    SubjectID = subjectId,
                };
                _translationRepository.SetNotiTitleAndBody(not);
                _context.Notis.Add(not);
                await _context.SaveChangesAsync();
                await _firebaseNotificationRepository.PushNotificationFromNoti(not);
            }
        }
        */

        public ICollection<Noti> GetLatest(int requester, int count)
        {
            var notis = _context.Notis.Where(n => n.ReceiverID == requester).OrderByDescending(n => n.SendTime).Take(count).ToList();
            return notis;
        }

        public async Task SetSeen(int requester, int id)
        {
            var notis = _context.Notis.Find(id);
            if (notis.Seen) throw new NothingUpdatedException();
            if (notis.ReceiverID != requester) throw new UnauthorizedAccessException();
            if (notis == null) throw new NotFoundException();
            notis.Seen = true;
            _context.Notis.Update(notis);
            await _context.SaveChangesAsync();
        }
    }

    public abstract class NotiReceiver { }

    public class UserReceiver : NotiReceiver
    {
        public int UserId { get; set; }
    }

    public class UsersManagersAndAdminsReceiver : NotiReceiver
    {
        public int UserId { get; set; }
    }

    public class Users : NotiReceiver
    {
        public ICollection<int> UserIds { get; set; }
        public Users(ICollection<int> _userIds)
        {
            UserIds = _userIds;
        }
    }
}