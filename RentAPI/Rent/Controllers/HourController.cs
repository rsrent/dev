using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HourController : Controller
    {
        private string ThisPermission => "Hour";

        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;

        public HourController(RentContext context, PermissionRepository permissionRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
        }

        [HttpGet("{locationID}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int locationID)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }
            var location = await _context.Location.Include(l => l.LocationHour).FirstOrDefaultAsync(l => l.ID == locationID);

            //var locationHour = await _context.LocationHour.FirstOrDefaultAsync(lh => lh.LocationID == locationID);
            return Ok(location?.LocationHour);
        }

        [HttpGet("Customer/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetForCustomer([FromRoute] int customerID)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            var customer = await _context.Customer.FindAsync(customerID);
            if (customer == null)
                return BadRequest();

            var locationHours = _context.Location.Include(l => l.LocationHour).Where(l => l.CustomerID == customerID).Select(l => l.LocationHour);

            //var locationHours = _context.LocationHour.Where(le => locations.Contains(le.LocationID));

            return Ok(locationHours);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] LocationHour locationHour)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update))
            {
                return Unauthorized();
            }
            _context.LocationHour.Update(locationHour);
            await _context.SaveChangesAsync();
            return Ok(locationHour);
        }
    }
}
