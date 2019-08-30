using System.Linq;
using System;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;
using Rent.DTOAssemblers;
using Rent.DTOs.TimePlanningDTO;
using Rent.Helpers;


namespace Rent.Controllers.TimePlanning
{

    [Produces("application/json")]
    [Route("api/Work")]

    public class WorkController : ControllerExecutor
    {

        private readonly WorkRepository _workRepository;


        public WorkController(WorkRepository workRepository)
        {
            _workRepository = workRepository;
        }

        [HttpGet("Get/{id}")]
        public IActionResult GetWork([FromRoute] int id)
        => Executor(() => _workRepository.GetDTO(Requester, id));

        [HttpGet("GetWorkOfUser/{userId}/{from}/{to}")]
        public IActionResult GetWorkOfUser([FromRoute] int userId, [FromRoute] DateTime from, [FromRoute] DateTime to)
        => Executor(() => _workRepository.GetAllWorkOfUserDTO(Requester, userId, from, to));

        [HttpGet("GetWorkOfUser/{userId}")]
        public IActionResult GetWorkOfUserTwoWeeks([FromRoute] int userId)
        => Executor(() => _workRepository.GetAllWorkOfUserDTO(Requester, userId));

        [HttpGet("GetWorkOfSignedInUser/{from}/{to}")]
        public IActionResult GetWorkOfSignedInUser([FromRoute] DateTime from, [FromRoute] DateTime to)
        => Executor(() => _workRepository.GetWorkOfSignedInUserDTO(Requester, from, to));

        [HttpGet("GetWorkOfSignedInUser")]
        public IActionResult GetWorkOfSignedInUserTwoWeeks()
        => Executor(() => _workRepository.GetWorkOfSignedInUserDTO(Requester));

        [HttpGet("GetWork/{from}/{to}")]
        public IActionResult GetWork([FromRoute] DateTime from, [FromRoute] DateTime to)
        => Executor(() => _workRepository.GetAllWorkDTO(Requester, from, to));

        [HttpGet("GetWork")]
        public IActionResult GetWorkTwoWeeks()
        => Executor(() => _workRepository.GetAllWorkDTO(Requester));

        [HttpGet("GetOfProjectItem/{projectItemId}/{from}/{to}")]
        public IActionResult GetOfProjectItem([FromRoute] int projectItemId, [FromRoute] DateTime from, [FromRoute] DateTime to)
        => Executor(() => _workRepository.GetWorkOfProjectItemDTO(Requester, projectItemId, from, to));

        [HttpGet("GetOfProjectItem/{projectItemId}")]
        public IActionResult GetOfProjectItem([FromRoute] int projectItemId)
        => Executor(() => _workRepository.GetWorkOfProjectItemDTO(Requester, projectItemId));

        [HttpGet("GetFreeWork")]
        public IActionResult GetFreeWork()
        => Executor(() => _workRepository.GetFreeWorkDTO(Requester));

        [HttpGet("GetLateWork")]
        public IActionResult GetLateWork()
        => Executor(() => _workRepository.GetLateWorkDTO(Requester));

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Work work)
        => await Executor(async () => await _workRepository.UpdateWork(Requester, work));

        [HttpPut("AddContract/{workId}/{contractId}")]
        public async Task<IActionResult> AddContract([FromRoute] int workId, [FromRoute] int contractId)
        => await Executor(async () => await _workRepository.AddContract(Requester, workId, contractId));

        [HttpPut("RemoveContract/{workId}")]
        public async Task<IActionResult> Update([FromRoute] int workId)
        => await Executor(async () => await _workRepository.RemoveContract(Requester, workId));

        [HttpPut("AddReplacer/{workId}/{contractId}")]
        public async Task<IActionResult> UpdateReplacer([FromRoute] int workId, [FromRoute] int contractId)
         => await Executor(async () => await _workRepository.AddReplacer(Requester, workId, contractId));

        [HttpPut("RemoveReplacer/{workId}")]
        public async Task<IActionResult> UpdateReplacer([FromRoute] int workId)
        => await Executor(async () => await _workRepository.RemoveReplacer(Requester, workId));


        [HttpPost("RegisterWork/{workId}")]
        public async Task<IActionResult> RegisterWork([FromRoute] int workId)
        => await Executor(async () =>
        {
            return await _workRepository.RegisterWork(Requester, workId);
        });


        [HttpPost("RegisterWork/{workId}/{startTime}/{endTime}")]
        public async Task<IActionResult> RegisterWorkWithCustomTime([FromRoute] int workId, [FromRoute] short startTime, [FromRoute] short endTime)
        => await Executor(async () =>
        {
            return await _workRepository.RegisterWork(Requester, workId, startTime, endTime);
        });

        [HttpPut("ReplyToRegistration/{registrationId}/{answer}")]
        public async Task<IActionResult> ReplyToRegistration([FromRoute] int registrationId, [FromRoute] bool answer)
        => await Executor(async () =>
        {
            await _workRepository.ReplyToRegistration(Requester, registrationId, answer);

        });

        [HttpPost("CreateForProjectItem/{projectItemId}")]
        public async Task<IActionResult> CreateForProjectItem([FromRoute] int projectItemId, [FromBody] Work work)
        => await Executor(async () => await _workRepository.CreateForProjectItem(Requester, work, projectItemId));

    }
}