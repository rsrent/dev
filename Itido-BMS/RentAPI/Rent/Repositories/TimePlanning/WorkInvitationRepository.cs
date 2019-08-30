using Rent.Data;
using System.Threading.Tasks;
using System;
using System.Linq;
using Rent.Models.TimePlanning;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using System.Collections.Generic;
using Rent.Models;

namespace Rent.Repositories.TimePlanning
{
    public class WorkInvitationRepository
    {
        private readonly RentContext _rentContext;
        private readonly RequestRepository _requestRepo;
        private readonly NotiRepository _notiRepo;
        private readonly WorkRepository _workRepository;
        private readonly AbsenceHelperRepository _absenceRepository;

        public WorkInvitationRepository(RentContext rentContext, RequestRepository requestRepo, NotiRepository notiRepo, WorkRepository workRepository, AbsenceHelperRepository absenceRepository)
        {
            _rentContext = rentContext;
            _requestRepo = requestRepo;
            _notiRepo = notiRepo;
            _workRepository = workRepository;
            _absenceRepository = absenceRepository;
        }

        //contract Id == null INGEN AFTAGER TIL VAGT
        //contract Id != null && contract.Replacement.ContractId == null AFTAGER MEN INGEN REPLACER
        //contract Id != null && contract.Replacement.ContractId != null AFTAGER OG AFLØSER
        public async Task CreateInvitation(int requester, int workId, int contractId)
        {
            var work = _rentContext.Work.Include(w => w.Contract).Include(w => w.WorkReplacement).FirstOrDefault(w => w.ID == workId);
            var contract = _rentContext.Contract.FirstOrDefault(c => c.ID == contractId);

            if (contract == null || work == null) throw new BadRequestException("Bad request 1");

            var inviteExist = _rentContext.WorkInvitation.Any(i => i.WorkID == workId && i.ContractID == contractId);
            var isUserOwnerOfWork = work.Contract != null && work.Contract.UserID == contract.UserID;

            Console.WriteLine("does invite exist?");
            Console.WriteLine(inviteExist);
            if (!inviteExist && !isUserOwnerOfWork && _absenceRepository.IsContractAvailableOnDate(contractId, work.Date))
            {
                Console.WriteLine("11111111111");
                if (work.ContractID == null)
                {
                    Console.WriteLine("222222222");

                    //Invite to not taken shift
                    var invitation = new WorkInvitation
                    {
                        WorkID = workId,
                        ContractID = contractId,
                    };

                    _rentContext.WorkInvitation.Add(invitation);
                    await _rentContext.SaveChangesAsync();
                    await _notiRepo.SendWorkInvitationNoti(requester, invitation);
                    return;
                }

                if (work.WorkReplacement != null && work.WorkReplacement.ContractID == null)
                {
                    Console.WriteLine("3333333333");

                    //Invite to shift as replacer
                    var invitation = new WorkInvitation
                    {
                        WorkID = workId,
                        ContractID = contractId,
                    };

                    _rentContext.WorkInvitation.Add(invitation);
                    await _rentContext.SaveChangesAsync();
                    await _notiRepo.SendWorkReplacementInviteNoti(requester, invitation);
                    return;
                }
            }
            Console.WriteLine("44444444");

            throw new BadRequestException("Bad request 4");

            throw new NotSupportedException();

        }

        public async Task ReplyToInvitation(int requester, int workId, bool answer)
        {
            var invitation = _rentContext.WorkInvitation.Include(i => i.Contract)
                .FirstOrDefault(i => i.WorkID == workId && i.Contract.UserID == requester);

            var work = _rentContext.Work.Include(w => w.WorkInvitations)
                .Include(w => w.WorkReplacement).FirstOrDefault(w => w.ID == invitation.WorkID);

            if (invitation == null || work == null) throw new NotFoundException();

            if (answer && _absenceRepository.IsContractAvailableOnDate(invitation.ContractID, work.Date))
            {
                //Send out invitation accepted noti
                //Delete all other invitations for that work
                if (work.WorkReplacement == null)
                {
                    work.ContractID = invitation.ContractID;
                }
                else
                {
                    work.WorkReplacement.ContractID = invitation.ContractID;
                }
                _rentContext.WorkInvitation.RemoveRange(work.WorkInvitations);
                await _rentContext.SaveChangesAsync();
                await _notiRepo.SendInvitationAcceptedNoti(requester, invitation);
            }
            else
            {
                //send out invition declined noti
                //Delete the invitation send to this user
                _rentContext.WorkInvitation.Remove(invitation);
                await _rentContext.SaveChangesAsync();
                await _notiRepo.SendInvitationDeclinedNoti(requester, invitation);

            }
        }
      
    }

}   