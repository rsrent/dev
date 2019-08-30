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
    [Route("api/CleaningDescriptions")]
    public class CleaningDescriptionsController : Controller
    {
        private readonly RentContext _context;

        public CleaningDescriptionsController(RentContext context)
        {
            _context = context;
        }

        // GET: api/CleaningDescriptions
        [HttpGet]
        public IEnumerable<CleaningDescription> GetCleaningDescription()
        {
            return _context.CleaningDescription;
        }

        // GET: api/CleaningDescriptions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCleaningDescription([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cleaningDescription = await _context.CleaningDescription.SingleOrDefaultAsync(m => m.ID == id);

            if (cleaningDescription == null)
            {
                return NotFound();
            }

            return Ok(cleaningDescription);
        }

        // PUT: api/CleaningDescriptions/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCleaningDescription([FromRoute] int id, [FromBody] CleaningDescription cleaningDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != cleaningDescription.ID)
            {
                return BadRequest();
            }

            _context.Entry(cleaningDescription).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CleaningDescriptionExists(id))
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

        // POST: api/CleaningDescriptions
        [HttpPost]
        public async Task<IActionResult> PostCleaningDescription([FromBody] CleaningDescription cleaningDescription)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.CleaningDescription.Add(cleaningDescription);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCleaningDescription", new { id = cleaningDescription.ID }, cleaningDescription);
        }

        // DELETE: api/CleaningDescriptions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCleaningDescription([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cleaningDescription = await _context.CleaningDescription.SingleOrDefaultAsync(m => m.ID == id);
            if (cleaningDescription == null)
            {
                return NotFound();
            }

            _context.CleaningDescription.Remove(cleaningDescription);
            await _context.SaveChangesAsync();

            return Ok(cleaningDescription);
        }

        private bool CleaningDescriptionExists(int id)
        {
            return _context.CleaningDescription.Any(e => e.ID == id);
        }
    }
}
*/