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
    public class EconomyController : Controller
    {
        private string ThisPermission => "Economy";

        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;

        public EconomyController(RentContext context, PermissionRepository permissionRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
        }

        [HttpGet("Location/{locationID}")]
        [Authorize]
        public async Task<IActionResult> GetLocation([FromRoute] int locationID)
        {
            if(_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }
            var location = await _context.Location.FindAsync(locationID);
            if (location == null)
                return BadRequest();
            /*
            var locationEconomy = await _context.LocationEconomy.FirstOrDefaultAsync(le => le.LocationID == locationID);
            if(locationEconomy == null)
            {
                locationEconomy = new LocationEconomy { LocationID = locationID };
                _context.LocationEconomy.Update(locationEconomy);
                await _context.SaveChangesAsync();
            }*/
            return Ok(_context.Location.Include(l => l.LocationEconomy).FirstOrDefault(l => l.ID == locationID)?.LocationEconomy);
        }

        [HttpGet("Customer/{customerID}")]
        [Authorize]
        public async Task<IActionResult> GetCustomer([FromRoute] int customerID)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }
            var customer = await _context.Customer.FindAsync(customerID);
            if (customer == null)
                return BadRequest();

            var locations = _context.Location.Include(l => l.LocationEconomy).Where(l => l.CustomerID == customerID).Select(l => l.LocationEconomy);

            //var locationEconomys = _context.LocationEconomy.Where(le => locations.Contains(le.LocationID));

            return Ok(locations);
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update([FromBody] LocationEconomy locationEconomy)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update))
            {
                return Unauthorized();
            }
            _context.LocationEconomy.Update(locationEconomy);
            await _context.SaveChangesAsync();
            return Ok();
        }

        int UserID => Int32.Parse(User.Claims.ToList()[0].Value);
        /*
        [HttpGet("Fix")]
        public async Task Fix()
        {
            var locations = _context.Location.ToList();

            foreach(var l in locations)
            {
                if(_context.LocationEconomy.FirstOrDefault(le => le.LocationID == l.ID) == null)
                {
                    _context.LocationEconomy.Add(new LocationEconomy{ LocationID = l.ID });
                }
            }
            _context.SaveChanges();
        } */
    }
}
