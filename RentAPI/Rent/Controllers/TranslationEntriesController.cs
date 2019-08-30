using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.DTOs;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/TranslationEntries")]
    public class TranslationEntriesController : Controller
    {
        private readonly RentContext _context;

        public TranslationEntriesController(RentContext context)
        {
            _context = context;
        }

        // GET: api/TranslationEntries
        [HttpGet]
        public IEnumerable<TranslationEntry> GetTranslationEntries()
        {
            return _context.TranslationEntry;
        }

        // GET: api/TranslationEntries/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTranslationEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var translationEntry = _context.TranslationEntry.Where(m => m.TranslationID == id);

            if (translationEntry == null)
            {
                return NotFound();
            }

            return Ok(translationEntry);
        }

        // PUT: api/TranslationEntries/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTranslationEntry([FromRoute] int id, [FromBody] TranslationEntry translationEntry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != translationEntry.ID)
            {
                return BadRequest();
            }

            _context.Entry(translationEntry).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TranslationEntryExists(id))
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

        // POST: api/TranslationEntries
        [HttpPost]
        public async Task<IActionResult> PostTranslationEntry([FromBody] TranslationEntryDTO translationEntryDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var translationToAdd = new Translation();
            var translation = await _context.Translation.AddAsync(translationToAdd);
            await _context.SaveChangesAsync();
            var translationEntry = new TranslationEntry
            {
                Language = translationEntryDTO.Language,
                Text = translationEntryDTO.Text,
                TranslationID = translationToAdd.ID
            };
            _context.TranslationEntry.Add(translationEntry);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTranslationEntry", new { id = translationEntry.ID }, translationEntry);
        }

        // DELETE: api/TranslationEntries/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTranslationEntry([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var translationEntry = await _context.TranslationEntry.SingleOrDefaultAsync(m => m.ID == id);
            if (translationEntry == null)
            {
                return NotFound();
            }

            _context.TranslationEntry.Remove(translationEntry);
            await _context.SaveChangesAsync();

            return Ok(translationEntry);
        }

        private bool TranslationEntryExists(int id)
        {
            return _context.TranslationEntry.Any(e => e.ID == id);
        }
    }
}