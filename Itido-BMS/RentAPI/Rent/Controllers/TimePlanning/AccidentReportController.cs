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
    [Route("api/AccidentReport")]
    public class AccidentReportController : ControllerExecutor
    {
        private readonly AccidentReportRepository _accidentReportRepository;
        public AccidentReportController(AccidentReportRepository accidentReportRepository)
        {
            _accidentReportRepository = accidentReportRepository;
        }

        [HttpGet("GetAllOfUser/{userId}")]
        public IActionResult GetAccidentReportOfUser([FromRoute] int userId)
        => Executor(() =>
        {
            var accidentReports = _accidentReportRepository.GetAllOfUser(Requester, userId);
            return accidentReports;
        });

        [HttpPost("Create/{userId}")]
        public async Task<IActionResult> Create([FromRoute] int userId, [FromBody] AccidentReport accidentReport)
        => await Executor(async () =>
        {
            return await _accidentReportRepository.Create(Requester, accidentReport, userId);
        });

        [HttpPut("Reply/{accidentReportId}/{answer}")]
        public async Task<IActionResult> Reply([FromRoute] int accidentReportId, [FromRoute] bool answer)
       => await Executor(async () => await _accidentReportRepository.ReplyToAccidentReport(Requester, accidentReportId, answer));
    }
}
