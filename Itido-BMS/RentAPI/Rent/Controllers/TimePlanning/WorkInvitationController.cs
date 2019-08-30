using System.Linq;
using System;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;
using Rent.DTOs.TimePlanningDTO;

namespace Rent.Controllers.TimePlanning
{
    [Produces("application/json")]
    [Route("api/WorkInvitation")]
    public class WorkInvitationController : ControllerExecutor
    {
        private readonly WorkInvitationRepository _invitationRepo;
        public WorkInvitationController(WorkInvitationRepository invitationRepo)
        {
            _invitationRepo = invitationRepo;
        }

        [HttpPost("Create/{workId}/{contractId}")]
        public async Task<IActionResult> Create([FromRoute] int workId, [FromRoute] int contractId)
        => await Executor(async () => await _invitationRepo.CreateInvitation(Requester, workId, contractId));

        [HttpPut("Update/{workId}/{answer}")]
        public async Task<IActionResult> Reply([FromRoute] int workId, [FromRoute] bool answer)
        => await Executor(async () => await _invitationRepo.ReplyToInvitation(Requester, workId, answer));


    }
}
