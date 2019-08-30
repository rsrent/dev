using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/PlanFloorArea")]
    public class PlanFloorAreaController : Controller
    {
        private readonly RentContext _context;

        public PlanFloorAreaController(RentContext context)
        {
            _context = context;
        }

        [HttpGet("Location/{locationID}")]
        public IActionResult Location([FromRoute] int locationID)
        {
            var item = _context.Location.Find(locationID);
            if (item == null) return NotFound("Location not found");
            return Ok(item.Name);
        }

        [HttpGet("Plan/{planID}")]
        public IActionResult Plan([FromRoute] int planID)
        {
            var item = _context.CleaningPlan.Find(planID);
            if (item == null) return NotFound("CleaningPlan not found");
            return Ok(item.Description);
        }

        [HttpGet("Floor/{floorID}")]
        public IActionResult Floor([FromRoute] int floorID)
        {
            var item = _context.Floor.Find(floorID);
            if (item == null) return NotFound("Floor not found");
            return Ok(item.Description);
        }

        [HttpGet("Plans")]
        public IActionResult Plans()
        {
            var plans = _context.CleaningPlan.ToList();
            return Ok(plans);
        }

        [HttpGet("Floors")]
        public IActionResult Floors()
        {
            var floors = _context.Floor.ToList();
            /*
            var floors = 
                _context.CleaningTask
                        .Include(c => c.Floor)
                        .GroupBy(c => c.Floor)
                        .OrderByDescending(c => c.Count())
                        .Select(g => g.Key).Where(f => f != null).ToList();

            var unusedFloors = _context.Floor.Where(f => !floors.Any(pf => pf.ID == f.ID));
            floors.AddRange(unusedFloors); */
            return Ok(floors);
        }

        [HttpGet("Areas/{cleaningPlanID}")]
        public IActionResult Areas([FromRoute] int cleaningPlanID)
        {
            var areas = _context.Area.Include(a => a.CleaningPlan).Where(a => a.CleaningPlanID == cleaningPlanID);
            /*
            var areas =
                _context.CleaningTask
                        .Include(c => c.Area)
                        .Where(a => a.Area.CleaningPlanID == cleaningPlanID)
                        .GroupBy(g => g.Area)
                        .OrderByDescending(c => c.Count())
                        .Select(c => c.Key).Where(a => a != null).ToList();

            var unusedAreas = _context.Area.Where(a => a.CleaningPlanID == cleaningPlanID && !areas.Any(pa => pa.ID == a.ID));
            areas.AddRange(unusedAreas); */

            return Ok(areas);
        }

        [HttpPost("AddFloor")]
        public async Task<IActionResult> AddFloor([FromBody] Floor floor)
        {
            _context.Floor.Add(floor);
            await _context.SaveChangesAsync();
            return Ok(floor);
        }

        [HttpPost("AddArea/{cleaningPlanID}")]
        public async Task<IActionResult> AddArea([FromRoute] int cleaningPlanID, [FromBody] Area area)
        {
            var cleaningPlan = _context.CleaningPlan.Find(cleaningPlanID);
            if(cleaningPlan == null) {
                return NotFound("CleaningPlan not found");
            }

            area.CleaningPlanID = cleaningPlanID;
            _context.Area.Add(area);
            await _context.SaveChangesAsync();
            area.CleaningPlan = cleaningPlan;
            return Ok(area);
        }
    }
}
