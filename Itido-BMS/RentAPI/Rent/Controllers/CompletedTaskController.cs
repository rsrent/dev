using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Helpers;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/CompletedTask")]
    public class CompletedTaskController : ControllerExecutor
    {
        private string ThisPermission = "CompletedTask";
        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;
        private readonly NotificationRepository _notificationRepository;
        private readonly PropCondition _propCondition;
        private readonly NewsRepository _newsRepository;
        private readonly TaskCompletedRepository _taskCompletedRepository;

        public CompletedTaskController(RentContext context, NotificationRepository notificationRepository, PermissionRepository permissionRepository, PropCondition propCondition, NewsRepository newsRepository, TaskCompletedRepository taskCompletedRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _notificationRepository = notificationRepository;
            _propCondition = propCondition;
            _newsRepository = newsRepository;
            _taskCompletedRepository = taskCompletedRepository;
        }

        // POST: api/QualityReports/Completed
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> TaskCompleted([FromBody] CleaningTaskCompleted cleaningTaskCompleted)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Create))
            {
                return Unauthorized();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var cleaningTask = _context.CleaningTask.Include("Area").FirstOrDefault(ct => ct.ID == cleaningTaskCompleted.CleaningTaskID);
            if (cleaningTask == null)
            {
                return BadRequest();
            }

            cleaningTaskCompleted.CompletedByUserID = Int32.Parse(User.Claims.ToList()[0].Value);
            _context.CleaningTaskCompleted.Add(cleaningTaskCompleted);

            await _context.SaveChangesAsync();

            if (cleaningTask.FirstCleaned == null)
            {
                cleaningTask.FirstCleaned = DateTimeHelpers.GmtPlusOneDateTime();
            }
            cleaningTask.LastTaskCompletedID = cleaningTaskCompleted.ID;
            _context.CleaningTask.Update(cleaningTask);
            _context.SaveChanges();

            _notificationRepository.TaskCompleted(cleaningTaskCompleted.ID);


            if (cleaningTask.Area.CleaningPlanID == 2)
                await _newsRepository.AddNews(Requester, cleaningTask.LocationID, Models.Important.NewsCategory.WindowTaskCompleted, cleaningTaskCompleted.ID);
            if (cleaningTask.Area.CleaningPlanID == 3)
                await _newsRepository.AddNews(Requester, cleaningTask.LocationID, Models.Important.NewsCategory.FanCoilTaskCompleted, cleaningTaskCompleted.ID);

            return Ok();
        }

        [HttpGet("{taskID}")]
        [Authorize]
        public IActionResult GetCompleted([FromRoute] int taskID)
        {
            if (_permissionRepository.Unauthorized(User, ThisPermission, CRUDD.Read))
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var completedTasks = _context.CleaningTaskCompleted
                .Where(ctc => ctc.CleaningTaskID == taskID)
                .Include(c => c.CleaningTask)
                .ThenInclude(a => a.Floor)
                .Include(c => c.CleaningTask)
                .ThenInclude(a => a.Area)
                .Include(c => c.CompletedByUser)
                                         .Select(ctc => ctc.Standard());

            return Ok(completedTasks);
        }

        [HttpPost("Create/{taskId}/{comment}")]
        public Task<IActionResult> Create(int taskId, string comment) =>
            Executor(() => _taskCompletedRepository.Create(Requester, taskId, comment));

        [HttpPost("Create/{taskId}")]
        public Task<IActionResult> Create(int taskId, [FromBody] CleaningTaskCompleted taskCompleted) =>
            Executor(() => _taskCompletedRepository.Create(Requester, taskId, taskCompleted));

        [HttpPut("Update/{taskCompletedId}")]
        public Task<IActionResult> Update(int taskCompletedId, [FromBody] CleaningTaskCompleted taskCompleted) =>
            Executor(() => _taskCompletedRepository.Update(Requester, taskCompletedId, taskCompleted));

        [HttpGet("Get/{taskCompletedId}")]
        public IActionResult Get(int taskCompletedId) =>
            Executor(() => _taskCompletedRepository.GetDTO(Requester, taskCompletedId));

        [HttpGet("GetOfTask/{taskId}")]
        public IActionResult GetOfTask(int taskId) =>
            Executor(() => _taskCompletedRepository.GetOfTaskDTO(Requester, taskId));
    }
}
