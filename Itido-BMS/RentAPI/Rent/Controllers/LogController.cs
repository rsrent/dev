using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Helpers;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class LogController : Controller
    {
        private readonly RentContext _context;

        public LogController(RentContext context)
        {
            _context = context;
        }




        [HttpPost("{locationID}")]
        [Authorize]
        public async Task<IActionResult> Add([FromRoute] int locationID)
        {
            var log = new LocationLog { UserID = UserID, LocationID = locationID, DateCreated = DateTimeHelpers.GmtPlusOneDateTime(), Log = "", CustomerCreated = false };
            _context.LocationLog.Add(log);
            await _context.SaveChangesAsync();
            //await _newsRepository.AddNews(UserID, log.LocationID, Models.Important.NewsCategory.LogCreated, log.ID);
            return Ok(log.ID);
        }

        [HttpPut("{logID}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int logID, [FromBody] LocationLogDTO logToUpdate)
        {
            var log = await _context.LocationLog.FindAsync(logID);
            if (!log.CustomerCreated)
            {
                log.Title = logToUpdate.Title;
                log.Log = logToUpdate.Log;
                _context.LocationLog.Update(log);
                await _context.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet("Many/{locationID}")]
        [Authorize]
        public async Task<IActionResult> GetMany([FromRoute] int locationID)
        {
            var user = _context.User.Find(UserID);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            if (user.CustomerID != null)
            {
                if (user.RoleID != 9)
                {
                    return BadRequest("User not customer manager");
                }

                Customer customer = await _context.Customer.FindAsync(user.CustomerID);

                if (customer == null)
                {
                    return BadRequest("Customer not found");
                }
                if (!customer.CanReadLogs)
                {
                    return BadRequest("Customer cant read logs");
                }
            }
            var logs = _context.LocationLog.Where(l => l.LocationID == locationID).OrderByDescending(l => l.DateCreated).Select(l => new { l.ID, l.Log, l.Title, l.DateCreated });
            return Ok(logs);
        }

        [HttpGet("{logID}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int logID)
        {
            var logs = _context.LocationLog.Find(logID);
            return Ok(logs);
        }

        int UserID => Int32.Parse(User.Claims.ToList()[0].Value);

        // PROJECT

        [HttpPost("CreateForProjectItem/{projectItemId}")]
        [Authorize]
        public async Task<IActionResult> CreateForProjectItem([FromRoute] int projectItemId)
        {
            var log = new LocationLog { UserID = UserID, ProjectItemID = projectItemId, DateCreated = DateTimeHelpers.GmtPlusOneDateTime(), Log = "", CustomerCreated = false };
            _context.LocationLog.Add(log);
            await _context.SaveChangesAsync();
            return Ok(log.ID);
        }

        [HttpGet("GetOfProjectItem/{projectItemId}")]
        public IActionResult GetOfProjectItem([FromRoute] int projectItemId)
        {
            var user = _context.User.Find(UserID);
            if (user == null)
            {
                return BadRequest("User not found");
            }
            var logs = _context.LocationLog.Where(l => l.ProjectItemID == projectItemId).OrderByDescending(l => l.DateCreated).Select(l => new { l.ID, l.Log, l.Title, l.DateCreated });
            return Ok(logs);
        }
    }
}
