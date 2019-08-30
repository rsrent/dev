using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;




namespace Rent.Controllers.TimePlanning
{
    [Produces("application/json")]
    [Route("api/AbsenceReason")]
    public class AbsenceReasonController : ControllerExecutor
    {
        private readonly AbsenceReasonRepository _absenceReasonRepository;


        public AbsenceReasonController(AbsenceReasonRepository absenceReasonRepository)
        {
            _absenceReasonRepository = absenceReasonRepository;

        }

        [HttpGet("Get/{id}")]
        public IActionResult Get([FromRoute] int id)
        => Executor(() => 
        { 
            return _absenceReasonRepository.Get(Requester, id);
        });

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        => Executor(() => 
        {
            return _absenceReasonRepository.GetAll(Requester);
        });


        [HttpPost("Create")]
        public async Task<IActionResult> CreateAbsenceReason([FromBody] AbsenceReason absenceReason)
        => await Executor(async () => await _absenceReasonRepository.CreateAbsenceReason(Requester, absenceReason));

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] AbsenceReason absenceReason)
        => await Executor(async () => await _absenceReasonRepository.UpdateAbsenceReason(Requester, absenceReason));



    }

}