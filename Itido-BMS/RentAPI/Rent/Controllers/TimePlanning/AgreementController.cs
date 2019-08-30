using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;


namespace Rent.Controllers.TimePlanning
{
     [Produces("application/json")]
    [Route("api/Agreement")]
    public class AgreementController : ControllerExecutor
    {
        private readonly IAgreementRepository _agreementRepository;
        public AgreementController(IAgreementRepository agreementRepository)
        {
            _agreementRepository = agreementRepository;
        }

        [HttpGet("Get/{id}")]
        public IActionResult Get([FromRoute] int id)
        => Executor(() =>
        {
             return _agreementRepository.Get(Requester, id);
        });

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        => Executor(() =>
        { 
            return _agreementRepository.GetAll(Requester);
        });

        [HttpPost("Create")]
        public async Task<IActionResult> CreateAgreement([FromBody] Agreement agreement)
        => await Executor(async () => await _agreementRepository.CreateAgreement(Requester, agreement));

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Agreement updatedAgreement)
        => await Executor(async () => await _agreementRepository.UpdateAgreement(Requester, updatedAgreement));


    }
}