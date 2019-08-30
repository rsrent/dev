using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    public class TermsController : Controller
    {
        private readonly RentContext _context;

        public TermsController(RentContext context)
        {
            _context = context;
        }
        /*
        [HttpGet]
        public async Task<IActionResult> GetUnacceptedTerms()
        {
            var userAcceptedTerms = _context.UserAcceptedTerm.Where(ut => ut.ID == _userID).ToList();
            var unacceptedTerms = _context.Term.Where(t => !userAcceptedTerms.Any(ut => ut.TermID == t.ID));
            return Ok(unacceptedTerms);
        }

        [HttpPost("termID")]
        public async Task<IActionResult> AcceptTerm([FromRoute]int termID)
        {
            _context.UserAcceptedTerm.Add(new UserAcceptedTerm { TermID = termID, UserID = _userID });
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("AddTerm")]
        public async Task<IActionResult> AddNewTerm([FromBody] Term newTerm)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            newTerm.CreatedByUserID = _userID;
            _context.Term.Add(newTerm);
            await _context.SaveChangesAsync();
            return Ok();
        }

        int _userID => Int32.Parse(User.Claims.ToList()[0].Value);
        */
    }
}
