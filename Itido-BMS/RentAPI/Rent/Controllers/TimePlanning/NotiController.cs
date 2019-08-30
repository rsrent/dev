using System.Linq;
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
    [Route("api/Noti")]
    //[AllowAnonymous]
    public class NotiController : ControllerExecutor
    {
        private readonly NotiRepository _notiRepository;
        public NotiController(NotiRepository notiRepository)
        {
            _notiRepository = notiRepository;
        }

        [HttpGet("GetLatest/{count}")]
        public IActionResult GetLatest([FromRoute] int count)
        => Executor(() =>
        {
            var notis = _notiRepository.GetLatest(Requester, count);
            return notis;
        });

        [HttpPut("SetSeen/{id}")]
        public Task<IActionResult> SetSeen([FromRoute] int id)
        =>  Executor(async () =>
        {
            await _notiRepository.SetSeen(Requester, id);
        });
    }
}
