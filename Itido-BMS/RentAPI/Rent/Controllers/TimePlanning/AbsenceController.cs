using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;
using Rent.DTOAssemblers;
using Rent.DTOs.TimePlanningDTO;
using System;

namespace Rent.Controllers.TimePlanning
{
    [Produces("application/json")]
    [Route("api/Absence")]
    public class AbsenceController : ControllerExecutor
    {
        private readonly IAbsenceRepository _absenceRepository;
        private readonly AbsenceAssembler _absenceAssembler;
        public AbsenceController(IAbsenceRepository absenceRepository, AbsenceAssembler absenceAssembler)
        {
            _absenceRepository = absenceRepository;
            _absenceAssembler = absenceAssembler;
        }

        [HttpGet("{absenceId}")]
        public IActionResult GetAbsence([FromRoute] int absenceId)
        => Executor(() =>
        {
            Console.WriteLine("Called");
            var absence = _absenceRepository.GetDTO(Requester, absenceId);
            return absence;
        });


        [HttpGet("GetAllOfUser/{userId}")]
        public IActionResult GetAbsenceOfUser([FromRoute] int userId)
        => Executor(() =>
        {
            Console.WriteLine("Called");
            var absences = _absenceRepository.GetAllAbsenceOfUserDTO(Requester, userId);
            return absences;
        });
        [HttpPost("Create/{userId}/{absenceReasonId}/{isRequest}")]
        public async Task<IActionResult> Create([FromRoute] int userId, [FromRoute] int absenceReasonId, [FromRoute] bool isRequest, [FromBody] AbsenceDTO absenceDTO)
        => await Executor(async () =>
        {
            var absence = _absenceAssembler.CreateAbsenceFromDTO(Requester, absenceDTO);
            return await _absenceRepository.CreateAbsence(Requester, absence, userId, absenceReasonId, isRequest);
        });

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] Absence updatedAbsence)
        => await Executor(async () => await _absenceRepository.UpdateAbsence(Requester, updatedAbsence));

        [HttpPut("Reply/{absenceId}/{answer}")]
        public async Task<IActionResult> Reply([FromRoute] int absenceId, [FromRoute] bool answer)
       => await Executor(async () => await _absenceRepository.ReplyToAbsence(Requester, absenceId, answer));
    }

}