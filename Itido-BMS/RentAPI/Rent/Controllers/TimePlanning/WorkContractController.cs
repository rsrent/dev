using System.Linq;
using System;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;
using Rent.DTOAssemblers;
using Rent.DTOs.TimePlanningDTO;

namespace Rent.Controllers.TimePlanning
{
    [Produces("application/json")]
    [Route("api/WorkContract")]

    public class WorkContractController : ControllerExecutor
    {

        private readonly WorkContractRepository _workContractRepository;
        private readonly WorkContractListAssembler _workContractAssembler;
        public WorkContractController(WorkContractRepository workContractRepository, WorkContractListAssembler workContractAssembler)
        {
            _workContractRepository = workContractRepository;
            _workContractAssembler = workContractAssembler;
        }


        /*
        [HttpPost("Create/{locationId}")]
        public async Task<IActionResult> Create([FromRoute] int locationId, [FromBody] WorkContract workContract)
        => await Executor(async () =>
        {
            return await _workContractRepository.CreateWorkContract(Requester, workContract, locationId);
        });
        */

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] WorkContract workContract)
        => await Executor(async () =>
        {
            await _workContractRepository.UpdateWorkContract(Requester, workContract);
        });


        [HttpPut("AddContract/{workContractId}/{contractId}")]
        public async Task<IActionResult> UpdateAddUserToWorkContract([FromRoute] int workContractId, [FromRoute] int contractId)
        => await Executor(async () =>
        {
            await _workContractRepository.AddUserToWorkContract(Requester, workContractId, contractId);
        });


        [HttpPut("RemoveContract/{workContractId}")]
        public async Task<IActionResult> UpdateRemoveUserFromWorkContract([FromRoute] int workContractId)
        => await Executor(async () =>
        {
            await _workContractRepository.RemoveUserFromWorkContract(Requester, workContractId);
        });

        [HttpGet("Get/{workContractId}")]
        public IActionResult Get([FromRoute] int workContractId)
        => Executor(() =>
        {
            return _workContractRepository.GetWorkContractDTO(Requester, workContractId);
            //return _workContractAssembler.CreateWorkContractListDTODetailed(wc);
        });
        /*
        [HttpGet("GetWorkContractsOfLocation/{locationId}")]
        public IActionResult GetForLocation([FromRoute] int locationId)
        => Executor(() =>
        {
            var wc = _workContractRepository.GetWorkContractsForLocationDTO(Requester, locationId);
            return wc;
            //return _workContractAssembler.CreateWorkContractListDTOList(wc);
        });
        */

        [HttpGet("GetWorkContractsOfUser/{contractId}")]
        public IActionResult GetForUser([FromRoute] int contractId)
       => Executor(() =>
       {
           var wc = _workContractRepository.GetWorkContractsForUserDTO(Requester, contractId);
           return wc;
           //return _workContractAssembler.CreateWorkContractListDTOList(wc);
       });

        // PROJECT

        [HttpPost("CreateForProjectItem/{projectItemId}")]
        public async Task<IActionResult> CreateForProjectItem([FromRoute] int projectItemId, [FromBody] WorkContract workContract)
        => await Executor(async () =>
        {
            return await _workContractRepository.CreateForProjectItem(Requester, workContract, projectItemId);
        });

        [HttpGet("GetOfProjectItem/{projectItemId}")]
        public IActionResult GetOfProjectItem([FromRoute] int projectItemId)
        => Executor(() =>
        {
            var works = _workContractRepository.GetOfProjectItem(Requester, projectItemId);
            return works;
        });

    }

}