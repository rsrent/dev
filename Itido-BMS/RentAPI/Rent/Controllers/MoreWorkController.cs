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
    public class MoreWorkController : Controller
    {
        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;
        private readonly NotificationRepository _notificationRepository;
        private readonly NewsRepository _newsRepository;


        public MoreWorkController(RentContext context, PermissionRepository permissionRepository, NotificationRepository notificationRepository, NewsRepository newsRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _notificationRepository = notificationRepository;
            _newsRepository = newsRepository;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Add([FromBody] MoreWork morework)
        {
            morework.CreatedByUserID = UserID;
            if (morework.Hours != null)
                morework.ActualCompletedTime = DateTimeHelpers.GmtPlusOneDateTime();
            else morework.ActualCompletedTime = null;
            
            _context.MoreWork.Add(morework);
            await _context.SaveChangesAsync();

            if(morework.Hours != null)
            {
                _notificationRepository.MoreWorkCompletedd(morework.ID);

                await _newsRepository.AddNews(UserID, morework.LocationID, Models.Important.NewsCategory.MoreWorkCompleted, morework.ID);
            } else {
                _notificationRepository.MoreWorkOrdered(morework.ID);

                await _newsRepository.AddNews(UserID, morework.LocationID, Models.Important.NewsCategory.MoreWorkOrdered, morework.ID);
            }
            return Ok();
        }

        [HttpPut("{moreWorkID}/{hours}")]
        [Authorize]
        public async Task<IActionResult> Update([FromRoute] int moreWorkID, [FromRoute] float hours)
        {
            var morework = await _context.MoreWork.FindAsync(moreWorkID);
            if (morework == null)
                return NotFound();
            if(morework.Hours != null)
            {
                return BadRequest();
            }
            morework.Hours = hours;
            morework.ActualCompletedTime = DateTimeHelpers.GmtPlusOneDateTime();
            _context.Update(morework);
            await _context.SaveChangesAsync();
            _notificationRepository.MoreWorkCompletedd(morework.ID);

            await _newsRepository.AddNews(UserID, morework.LocationID, Models.Important.NewsCategory.MoreWorkCompleted, morework.ID);
            return Ok();
        }

        [HttpGet("{locationID}")]
        [Authorize]
        public async Task<IActionResult> Get([FromRoute] int locationID)
        {
            List<int> PlansToFetch = new List<int>();

            var stringPermissions = _permissionRepository.GetUserPermissions(0, UserID);
            foreach (var up in stringPermissions)
            {
                if (up.Name == "RegularTask" && up.Read)
                    PlansToFetch.Add(1);
                if (up.Name == "WindowTask" && up.Read)
                    PlansToFetch.Add(2);
                if (up.Name == "FanCoilTask" && up.Read)
                    PlansToFetch.Add(3);
            }

            var moreWork =
                _context.MoreWork
                        .Where(m => PlansToFetch.Contains(m.CleaningPlanID) && m.LocationID == locationID)
                        .OrderByDescending(m => m.ExpectedCompletedTime);
            return Ok(moreWork);
        }

        int UserID => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
