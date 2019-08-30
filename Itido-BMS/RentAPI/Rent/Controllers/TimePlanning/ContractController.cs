using System.Linq;
using System;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Rent.DTOs.TimePlanningDTO;
using Rent.DTOAssemblers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;



namespace Rent.Controllers.TimePlanning
{

    [Produces("application/json")]
    [Route("api/Contract")]
    public class ContractController : ControllerExecutor
    {
        private readonly ContractRepository _contractRepository;
        private readonly ContractAssembler _contractAssembler;
        public ContractController(ContractRepository contractRepository, ContractAssembler contractAssembler)
        {
            _contractRepository = contractRepository;
            _contractAssembler = contractAssembler;
        }

        [HttpPost("Create/{userId}/{agreementId}")]
        public async Task<IActionResult> CreateContract([FromRoute] int userId, [FromRoute] int agreementId, [FromBody] ContractDTO contractDTO)
        => await Executor(async () =>
        {
            var contract = _contractAssembler.CreateContract(contractDTO, userId, agreementId);
            await _contractRepository.CreateContract(Requester, contract);
        });


        //TODO GET ALL CONTRACTS OF USER
        [HttpGet("GetAllOfUser/{userId}")]
        public IActionResult GetContractsForUser([FromRoute] int userId)
        => Executor(() =>
        {
            Console.WriteLine("Called the contract controller");
            var result = _contractRepository.GetAllContractsForUser(Requester, userId);
            return result;
        });

        /* [HttpGet("{id}")]
        public IActionResult GetContractsForUserAs([FromRoute] int userId)
        
        => Executor(() => {
            var result = _contractRepository.GetAllContractsForUserAs(userId, _contractAssembler.CreateContractDTO);
            return result;
        });*/

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ContractDTO updatedContract)
        => await Executor(async () =>
        {
            var contract = _contractAssembler.CreateContract(updatedContract, 0, 0);
            await _contractRepository.UpdateContract(Requester, contract);
        });


        [HttpGet("Get/{contractId}")]
        public IActionResult Get([FromRoute] int contractId)
        => Executor(() =>
        {
            var result = _contractRepository.Get(Requester, contractId);
            return result;
        });

    }


}