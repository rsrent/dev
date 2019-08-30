using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using Rent.Helpers;
using Rent.Models;
using Rent.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Rating")]
    public class RatingController : Controller
    {
        private string ThisPermission = "Rating";
        private readonly RentContext _context;
        private readonly RatingRepository _ratingRepository;
        private readonly PermissionRepository _permissionRepository;
        private readonly NotificationRepository _notificationRepository;
        private readonly NewsRepository _newsRepository;
        private readonly SendGridEmail _sendGridEmail;

        public RatingController(RentContext context, RatingRepository ratingRepository, NotificationRepository notificationRepository, PermissionRepository permissionRepository, NewsRepository newsRepository, SendGridEmail sendGridEmail)
        {
            _context = context;
            _ratingRepository = ratingRepository;
            _permissionRepository = permissionRepository;
            _notificationRepository = notificationRepository;
            _newsRepository = newsRepository;
            _sendGridEmail = sendGridEmail;
        }

        [HttpPost("AddRatingItem/{ratingID}/{title}/{value}")]
        [Authorize]
        public async Task<IActionResult> AddRatingItemToRating([FromRoute] int ratingID, [FromRoute] string title, [FromRoute] int value, [FromBody] string comment)
        {
            /*   
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Create))
            {
                return Unauthorized();
            }
            */

            var rating = _context.Rating.Find(ratingID);
            if (rating == null)
                return NotFound("Rating not found");


            var ratingItem = new RatingItem
            {
                Comment = comment,
                RatedByID = Int32.Parse(User.Claims.ToList()[0].Value),
                Value = value,
                RatingID = ratingID,
                Title = title,
                TimeRated = DateTimeHelpers.GmtPlusOneDateTime(),
            };

            _context.RatingItem.Add(ratingItem);
            await _context.SaveChangesAsync();

            var qrp = _context.QualityReport.FirstOrDefault(q => q.RatingID == ratingID);
            if(qrp != null)
            {
                var header = "Ny vurdering af samarbejde";
                var body = "Kommentar:\n" + comment + "\nRating:\n" + (value == 1 ? "God" : value == 2 ? "Okay" : "Dårlig");

                await _newsRepository.AddNews(_userID, qrp.LocationID,header, body);

                /*

                var locationUser = _context.LocationUser.Include(lu => lu.User).FirstOrDefault(lu => lu.Location.ID == qrp.LocationID && lu.User.RoleID == 3);

                if (locationUser != null && locationUser.User.Email != null)
                {
                    try
                    {
                        await _sendGridEmail.Send(locationUser.User.Email, header, body);
                    }
                    catch (Exception e) { }
                }
                */
            }
            return Ok(true);
        }
        /*
        [HttpGet("{locationID}")]
        [Authorize]
        public async Task<IActionResult> GetRatingsForLocation([FromRoute] int locationID)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }
            var ratings = await _ratingRepository.GetRatingsForLocation(locationID);

            if (ratings == null)
                return BadRequest();

            return Ok(ratings);
        }*/

        int _userID => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
