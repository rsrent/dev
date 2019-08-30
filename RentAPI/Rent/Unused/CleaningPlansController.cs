//TODO REMOVE
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Newtonsoft.Json;
using Rent.DTOs;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/CleaningPlans")]
    public class CleaningPlansController : Controller
    {
        private readonly RentContext _context;

        public CleaningPlansController(RentContext context)
        {
            _context = context;
        }

        // GET: api/CleaningPlans
        //[HttpGet]
        //public IEnumerable<CleaningPlan> GetCleaningPlan()
        //{
        //    return _context.CleaningPlan.Include(c => c.CleaningTasks);
        //}

        // GET: api/CleaningPlans/CustomerID/5
        /// <summary>
        /// Gets the cleaningplan for the given location.
        /// </summary>
        /// <param name="id">ID of the location</param>
        /// <returns>A cleaningplan</returns>
        /// <response code="200">Returns the cleaningplan.</response>
        /// <response code="404">If there is no cleaningplan for the given location.</response>            
        [HttpGet("LocationID/{id}")]
        public async Task<IActionResult> GetCleaningPlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cleaningPlan = await _context.Location
                .Include(c => c.CleaningTasks)
                .ThenInclude(a => a.Floor)
                .Include(c => c.CleaningTasks)
                .ThenInclude(a => a.Area).Include(c => c.CleaningTasks)
                .SingleOrDefaultAsync(m => m.ID == id);

            var tasks = _context.CleaningTask.Where(ct => ct.LocationID == id).Include(f => f.Floor).Include(a => a.Area).Select(ct => 
            new CleaningTaskDTO {
                Area = ct.Area, 
                Floor = ct.Floor,
                TimesCleanedThisYear = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID && ctc.CompletedDate.Year.Equals(DateTime.Now.Year)).Count(),
                LastTaskCompleted = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID).OrderByDescending(ctc => ctc.CompletedDate).First(),
                Comment = ct.Comment,
                Interval = ct.Interval,
                PlanType = ct.PlanType,
                SquareMeters = ct.SquareMeters,
                Frequency = ct.Frequency,
                ID = ct.ID
            });


              //.Include(c => c.CleaningTasks)
                //.ThenInclude(a => a.CleaningTask.Floor)
                //.Include(c => c.CleaningTasks)
                //.ThenInclude(a => a.CleaningTask.Area)

            if (cleaningPlan == null)
            {
                return NotFound();
            }

            return Ok(cleaningPlan);
        }

        // PUT: api/CleaningPlans/AddCleaningTask/5
        [HttpPut("AddCleaningTask/{id}")]
        public async Task<IActionResult> AddCleaningTaskToPlan([FromRoute] int id, [FromBody] CleaningTask CleaningTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var cleaningPlan = _context.CleaningPlan.Find(id);
            if (_context.Area.Find(CleaningTask.AreaID) == null)
            {
                var area = await _context.Area.FirstOrDefaultAsync(x => x.Description.Equals(CleaningTask.Area.Description));
                if (area != null)
                {   
                    CleaningTask.Area = area;
                }
                else
                {
                    await _context.Area.AddAsync(CleaningTask.Area);
                }
            }
            if (_context.Floor.Find(CleaningTask.FloorID) == null)
            {
                var floor = await _context.Floor.FirstOrDefaultAsync(x => x.Description.Equals(CleaningTask.Floor.Description));
                if (floor != null)
                {
                    CleaningTask.Floor = floor;
                }
                else
                {
                    await _context.Floor.AddAsync(CleaningTask.Floor);
                }
            }
            await _context.SaveChangesAsync();
            CleaningTask.CleaningPlanID = id;
            //CleaningTask.FloorID = CleaningTask.Floor.ID;
            //CleaningTask.AreaID = CleaningTask.Area.ID;
            _context.CleaningTask.Add(CleaningTask);

            _context.Entry(cleaningPlan).State = EntityState.Modified;


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleaningPlanExists(id))
                {
                    return NotFound("Not found");
                }
                else
                {
                    return NotFound("Kartoffel");
                }
            }

            return NoContent();
            return BadRequest();
        }

        // PUT: api/CleaningPlans/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCleaningPlan([FromRoute] int id, [FromBody] CleaningPlan cleaningPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cleaningPlan.ID)
            {
                return BadRequest();
            }

            _context.Entry(cleaningPlan).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleaningPlanExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CleaningPlans
        [HttpPost]
        public async Task<IActionResult> PostCleaningPlan([FromBody] CleaningPlan cleaningPlan)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CleaningPlan.Add(cleaningPlan);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCleaningPlan", new { id = cleaningPlan.ID }, cleaningPlan);
        }

        // DELETE: api/CleaningPlans/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleaningPlan([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cleaningPlan = await _context.CleaningPlan.SingleOrDefaultAsync(m => m.ID == id);
            if (cleaningPlan == null)
            {
                return NotFound();
            }

            _context.CleaningPlan.Remove(cleaningPlan);
            await _context.SaveChangesAsync();

            return Ok(cleaningPlan);
        }

        private bool CleaningPlanExists(int id)
        {
            return _context.CleaningPlan.Any(e => e.ID == id);
        }
    }
}
*/