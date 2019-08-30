using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/CleaningTask")]
    public class CleaningTaskController : ControllerExecutor
    {
        //private string ThisPermission = "CleaningTask";
        //private readonly RentContext _context;
        //private readonly PermissionRepository _permissionRepository;
        //private readonly NotificationRepository _notificationRepository;
        private readonly CleaningTaskRepository _cleaningTaskRepository;

        public CleaningTaskController(/*RentContext context, NotificationRepository notificationRepository, PermissionRepository permissionRepository, */CleaningTaskRepository cleaningTaskRepository, ProjectRepository projectRepository)
        {
            //_context = context;
            //_permissionRepository = permissionRepository;
            //_notificationRepository = notificationRepository;
            _cleaningTaskRepository = cleaningTaskRepository;
        }

        [HttpGet("Plans/{locationID}")]
        //[Authorize]
        public IActionResult Plans([FromRoute] int locationID)
        {
            return Executor(() => _cleaningTaskRepository.Plans(Requester, locationID));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
                return Unauthorized();

            if (_context.Location.Find(locationID) == null)
                return NotFound("Location not found");

            var planTypes = _cleaningTaskRepository.PlansForLocation(locationID, _userID);

            return Ok(planTypes); */
        }

        [HttpGet("Floors/{locationID}/{planID}")]
        //[Authorize]
        public IActionResult Floors([FromRoute] int locationID, [FromRoute] int planID)
        {
            return Executor(() => _cleaningTaskRepository.Floors(Requester, locationID, planID));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
                return Unauthorized();

            if(_context.Location.Find(locationID) == null) 
                return NotFound("Location not found");

            var floors = new List<CleaningPlanFloorDTO>();
            var tasks = _cleaningTaskRepository.GetTasks(locationID, _userID).Where(t => t.Area.CleaningPlanID == planID).ToHashSet();
            foreach (var task in tasks)
            {
                if(task.Floor == null)
                {
                    return BadRequest("Der er ikke nogle etager, til denne plan");
                }
                if (!floors.Any(f => f.Floor.ID == task.Floor.ID))
                    floors.Add(new CleaningPlanFloorDTO { Floor = task.Floor, Areas = new List<CleaningPlanAreaDTO>() });
                var floor = floors.FirstOrDefault(f => f.Floor.ID == task.Floor.ID);

                if (!floor.Areas.Any(a => a.Area.ID == task.Area.ID))
                    floor.Areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                floor.Areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());
            }
            return Ok(floors);*/
        }

        [HttpGet("TasksFromFloor/{locationID}/{planID}/{floorID}")]
        //[Authorize]
        public IActionResult TaskFromFloor([FromRoute] int locationID, [FromRoute] int planID, [FromRoute] int floorID)
        {
            return Executor(() => _cleaningTaskRepository.Tasks(Requester, locationID, planID, floorID));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
                return Unauthorized();

            if (_context.Location.Find(locationID) == null)
                return NotFound("Location not found");

            var areas = new List<CleaningPlanAreaDTO>();
            foreach(var task in _cleaningTaskRepository.GetTasks(locationID, _userID).Where(t => t.Area.CleaningPlanID == planID && t.Floor.ID == floorID).ToHashSet())
            {
                if (!areas.Any(a => a.Area.ID == task.Area.ID))
                    areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());
            }

            return Ok(areas); */
        }

        [HttpGet("TasksFromPlan/{locationID}/{planID}")]
        //[Authorize]
        public IActionResult TaskFromPlan([FromRoute] int locationID, [FromRoute] int planID)
        {
            return Executor(() => _cleaningTaskRepository.Tasks(Requester, locationID, planID));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
                return Unauthorized();

            if (_context.Location.Find(locationID) == null)
                return NotFound("Location not found");

            var areas = new List<CleaningPlanAreaDTO>();
            foreach (var task in _cleaningTaskRepository.GetTasks(locationID, _userID).Where(t => t.Area.CleaningPlanID == planID).ToHashSet())
            {
                if (!areas.Any(a => a.Area.ID == task.Area.ID))
                    areas.Add(new CleaningPlanAreaDTO { Area = task.Area, Tasks = new List<dynamic>() });
                areas.FirstOrDefault(a => a.Area.ID == task.Area.ID).Tasks.Add(task.Standard());
            }

            return Ok(areas); */
        }

        [HttpPost("Add/{locationID}")]
        //[Authorize]
        public async Task<IActionResult> PostCleaningTaskToPlan([FromRoute] int locationID, [FromBody] CleaningTask CleaningTask)
        {
            return await Executor(async () => await _cleaningTaskRepository.Add(Requester, locationID, CleaningTask));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Create))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}
            var location = _context.Location.Find(locationID);
            if(location == null) {
                return NotFound("Location not found");
            }
            Floor floor = null;
            if(CleaningTask.Floor != null) {
                floor = _context.Floor.Find(CleaningTask.Floor.ID);
                if(floor == null) {
                    return NotFound("Floor not found");
                }
            }
            var area = _context.Area.Find(CleaningTask.Area.ID);
            if(area == null) {
                return NotFound("Area not found");
            }

            var newTask = new CleaningTask
            {
                AreaID = area.ID,
                FloorID = floor?.ID,
                Comment = CleaningTask.Comment,
                Frequency = CleaningTask.Frequency,
                LocationID = locationID,
                SquareMeters = CleaningTask.SquareMeters,
                Active = true,
                TimesOfYear = CleaningTask.TimesOfYear, 
            };

            _context.CleaningTask.Add(newTask);
            _context.Entry(location).State = EntityState.Modified;
            //var theNewTask = _context.CleaningTask.Include(c => c.Area).FirstOrDefault(t => t.ID == newTask.ID);

			try
			{
				await _context.SaveChangesAsync();
			}
			catch (DbUpdateConcurrencyException)
			{
                return BadRequest();
			}

            if (newTask.Area.CleaningPlanID == 1)
            {
                var newQualityReportItems = _context.QualityReport.Where(q => q.LocationID == locationID && q.RatingID == null).Select(q => new QualityReportItem { CleaningTaskID = newTask.ID, QualityReportID = q.ID });
                _context.QualityReportItem.AddRange(newQualityReportItems);
                await _context.SaveChangesAsync();
            }

            return Ok(newTask); */
        }


        // PUT: api/CleaningTask/
        /*
		[HttpPut]
        [Authorize]
        public async Task<IActionResult> PutCleaningTask([FromBody] CleaningTask updatedCleaningTask)
		{
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Update))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

            var cleaningTask = _context.CleaningTask.Find(updatedCleaningTask.ID);
            if(cleaningTask == null) {
                return NotFound();
            }

            _context.Entry(updatedCleaningTask).State = EntityState.Modified;
            await _context.SaveChangesAsync();
			
			return NoContent();
		} */


        // DELETE: api/CleaningTask/5
        [HttpDelete("Delete/{id}")]
        //[Authorize]
        public async Task<IActionResult> DeleteCleaningTask([FromRoute] int id)
        {
            return await Executor(async () => await _cleaningTaskRepository.Delete(Requester, id));
            /*
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Delete))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
			{
				return BadRequest(ModelState);
			}

			var CleaningTask = _context.CleaningTask.Find(id);
			if (CleaningTask == null)
			{
				return NotFound();
			}
            var locationID = CleaningTask.LocationID;

            CleaningTask.Active = false;
			_context.CleaningTask.Update(CleaningTask);
			await _context.SaveChangesAsync();

            var qualityReportItems = _context.QualityReportItem.Include(q => q.QualityReport).Where(q => q.CleaningTaskID == CleaningTask.ID && q.QualityReport.RatingID == null);

            _context.QualityReportItem.RemoveRange(qualityReportItems);
            await _context.SaveChangesAsync();

            return Ok(CleaningTask); */
        }

        /*

        List<int> UsersToNotify(int locationID, int planNameID)
        {
            string planName = "";
            if (planNameID == 0)
                planName = "RegularTask";
            if (planNameID == 1)
                planName = "WindowTask";
            if (planNameID == 2)
                planName = "FanCoilTask";


            var usersToNotify = new List<int>();
            var location = _context.Location.Find(locationID);

            //Customer main user
            var customerUsers = _context.Customer.Where(c => c.ID == location.CustomerID && c.MainUserID != null).Select(c => (int)c.MainUserID).ToList();
            if (customerUsers != null)
                usersToNotify.AddRange(customerUsers);

            //Location users
            var locationUsers = _context.LocationUser.Include(lu => lu.User).Where(lu => lu.LocationID == locationID).Select(lu => lu.UserID).ToList();

            if(planName != null) {

                locationUsers = locationUsers.Where(id => _context.UserPermissions
                            .Include(up => up.Permission)
                            .Where(up => up.UserID == id)
                            .Any(up => planName.ToLower() == up.Permission.Name.ToLower() && up.Read == true)).ToList();
            }

            if (locationUsers != null)
                usersToNotify.AddRange(locationUsers);

            return usersToNotify;
        }





        // TODO REMOVE
        [HttpGet("GetDictionaryTest/{locationID}")]
        public IActionResult GetDictionaryTest([FromRoute] int locationID)
        {
            var tasks = _context.CleaningTask.Where(ct => ct.LocationID == locationID).Include(f => f.Floor).Include(a => a.Area).Select(ct =>
            new CleaningTaskDTO
            {
                Area = ct.Area,
                Floor = ct.Floor,
                TimesCleanedThisYear = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID && ctc.CompletedDate.Year == DateTime.Now.Year).Count(),
                LastTaskCompleted = _context.CleaningTaskCompleted.Where(ctc => ctc.CleaningTaskID == ct.ID).OrderByDescending(ctc => ctc.CompletedDate).FirstOrDefault(),
                Comment = ct.Comment,
                TimesOfYear = ct.TimesOfYear,
                PlanType = ct.PlanType,
                SquareMeters = ct.SquareMeters,
                Frequency = ct.Frequency,
                ID = ct.ID
            });




            var dic = new Dictionary<string, Dictionary<string, Dictionary<string, List<CleaningTaskDTO>>>>();

            foreach (var item in tasks)
            {
                var planType = item.PlanType.ToString();
                var floor = item.Floor.Description;
                var area = item.Area.Description;

                if (!dic.ContainsKey(planType))
                    dic.Add(planType, new Dictionary<string, Dictionary<string, List<CleaningTaskDTO>>>());
                if (!dic[planType].Keys.Contains(floor))
                    dic[planType].Add(floor, new Dictionary<string, List<CleaningTaskDTO>>());
                if (!dic[planType][floor].Keys.Contains(area))
                    dic[planType][floor].Add(area, new List<CleaningTaskDTO>());
                dic[planType][floor][area].Add(item);
            }

            return Ok(dic);
        }
        */



        //TODO make this work
        /*
                [HttpGet("Test")]
                [AllowAnonymous]
                public IActionResult Test()
                {
                    var untill = DateTime.Now.AddDays(7);

                    var res = _cleaningTaskRepository.Test2(1).ToList();

                    //foreach(var r in res)
                    //    r.NextCleaned = r.LastTaskCompleted.CompletedDate.AddDays((int)(365.0 / (int)r.TimesOfYear));

                    return Ok(res.Where(ct => ct.NextCleaned < untill).OrderBy(ct => ct.NextCleaned));


                    //var test = _context.CleaningTask.Include(ct => ct.CompletedTasks).Select(ct => ct.CompletedTasks.Count() > 0 ? ct.CompletedTasks.Last() : null);
                    //return Ok(test);
                }
        */

        [HttpGet("Get/{taskId}")]
        public IActionResult AddTask(int taskId)
        => Executor(() => _cleaningTaskRepository.Get(Requester, taskId));



        [HttpPut("Update/{taskId}")]
        public Task<IActionResult> Update(int taskId, [FromBody] CleaningTask cleaningTask)
        => Executor(() => _cleaningTaskRepository.Update(Requester, taskId, cleaningTask));


        // PROJECT

        [HttpGet("GetOfProjectItem/{projectItemId}")]
        public IActionResult GetTasksOfProjectItem(int projectItemId)
        => Executor(() => _cleaningTaskRepository.GetTasksOfProjectItem(Requester, projectItemId));

        [HttpPost("CreateForProjectItem/{projectItemId}")]
        public Task<IActionResult> Create(int projectItemId, [FromBody] CleaningTask cleaningTask)
        => Executor(async () => await _cleaningTaskRepository.Create(Requester, projectItemId, cleaningTask));
    }
}