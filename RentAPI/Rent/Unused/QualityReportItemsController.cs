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
    [Route("api/QualityReportItems")]
    public class QualityReportItemsController : Controller
    {
        private readonly RentContext _context;

        public QualityReportItemsController(RentContext context)
        {
            _context = context;
        }

        // GET: api/QualityReportItems
        [HttpGet]
        public IEnumerable<QualityReportItem> GetQualityReportItem()
        {
            return _context.QualityReportItem;
        }

        // GET: api/QualityReportItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetQualityReportItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qualityReportItem = await _context.QualityReportItem.SingleOrDefaultAsync(m => m.ID == id);

            if (qualityReportItem == null)
            {
                return NotFound();
            }

            return Ok(qualityReportItem);
        }

        // PUT: api/QualityReportItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutQualityReportItem([FromRoute] int id, [FromBody] QualityReportItem qualityReportItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != qualityReportItem.ID)
            {
                return BadRequest();
            }

            _context.Entry(qualityReportItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!QualityReportItemExists(id))
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

        // POST: api/QualityReportItems
        [HttpPost]
        public async Task<IActionResult> PostQualityReportItem([FromBody] QualityReportItem qualityReportItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.QualityReportItem.Add(qualityReportItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetQualityReportItem", new { id = qualityReportItem.ID }, qualityReportItem);
        }

        // DELETE: api/QualityReportItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteQualityReportItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var qualityReportItem = await _context.QualityReportItem.SingleOrDefaultAsync(m => m.ID == id);
            if (qualityReportItem == null)
            {
                return NotFound();
            }

            _context.QualityReportItem.Remove(qualityReportItem);
            await _context.SaveChangesAsync();

            return Ok(qualityReportItem);
        }

        private bool QualityReportItemExists(int id)
        {
            return _context.QualityReportItem.Any(e => e.ID == id);
        }
    }
}
*/