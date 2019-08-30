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

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Floors")]
    public class FloorsController : Controller
    {
        private readonly RentContext _context;

        public FloorsController(RentContext context)
        {
            _context = context;
        }

        // GET: api/Floors
        [HttpGet]
        public IEnumerable<Floor> GetFloor()
        {
            return _context.Floor;
        }

        // GET: api/Floors/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetFloor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var floor = await _context.Floor.SingleOrDefaultAsync(m => m.ID == id);

            if (floor == null)
            {
                return NotFound();
            }

            return Ok(floor);
        }

        // PUT: api/Floors/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFloor([FromRoute] int id, [FromBody] Floor floor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != floor.ID)
            {
                return BadRequest();
            }

            _context.Entry(floor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FloorExists(id))
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

        // POST: api/Floors
        [HttpPost]
        public async Task<IActionResult> PostFloor([FromBody] Floor floor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Floor.Add(floor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFloor", new { id = floor.ID }, floor);
        }

        // DELETE: api/Floors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFloor([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var floor = await _context.Floor.SingleOrDefaultAsync(m => m.ID == id);
            if (floor == null)
            {
                return NotFound();
            }

            _context.Floor.Remove(floor);
            await _context.SaveChangesAsync();

            return Ok(floor);
        }

        private bool FloorExists(int id)
        {
            return _context.Floor.Any(e => e.ID == id);
        }
    }
}
*/