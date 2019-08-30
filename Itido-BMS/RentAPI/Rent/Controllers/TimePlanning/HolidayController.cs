using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories.TimePlanning;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Microsoft.AspNetCore.Authorization;


namespace Rent.Controllers.TimePlanning
{

    [Produces("application/json")]
    [Route("api/Holiday")]

    public class HolidayController : ControllerExecutor
    {
        private readonly HolidayRepository _holidayRepository;

        public HolidayController(HolidayRepository holidayRepository)
         {
             _holidayRepository = holidayRepository;
         }

        [HttpGet("GetAllOfCountryCode/{countryCode}")]
        public IActionResult GetContractsForUser([FromRoute] string countryCode)
        => Executor(() => {
          return _holidayRepository.GetAllHolidaysForCountryCode(Requester, countryCode);
        });
    }


}