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
using Microsoft.AspNetCore.Authorization;
using Rent.Repositories;
using Rent.ContextPoint.Exceptions;
using Microsoft.AspNetCore.Authorization;


namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Users")]
    public class UsersController : ControllerExecutor
    {
        private readonly UserRepository _userRepository;

        public UsersController(UserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet("{userId}")]
        public IActionResult GetUser([FromRoute] int userId)
        {
            return Executor(() => _userRepository.Get(Requester, userId));
        }

        [HttpPut("Update/{userId}")]
        public async Task<IActionResult> GetUser([FromRoute] int userId, [FromBody] User user)
        {
            return await Executor(async () => await _userRepository.UpdateUser(Requester, userId, user));
        }

        [HttpPut("UpdateUserImage/{userID}/{imageLocator}")]
        public async Task<IActionResult> UpdateUserImage([FromRoute] int userID, [FromRoute] string imageLocator)
        => await Executor(async () => await _userRepository.UpdateUserImage(Requester, userID, imageLocator));

        [HttpGet]
        public IActionResult GetAll()
        {
            return Executor(() => _userRepository.GetAll(Requester));
        }

        [HttpGet("WithRole/{roleId}")]
        public IActionResult GetUsersWithRole([FromRoute] int roleId)
        {
            return Executor(() => _userRepository.GetWithRole(Requester, roleId));
        }

        [HttpGet("GetLocationUsers/{locationId}")]
        public IActionResult GetLocationUsers([FromRoute] int locationId)
        {
            return Executor(() => _userRepository.GetLocationUsers(Requester, locationId));
        }

        [HttpGet("GetCustomerUsers/{customerId}")]
        public IActionResult GetCustomerUsers([FromRoute] int customerId)
        {
            return Executor(() => _userRepository.GetCustomerUsers(Requester, customerId));
        }

        [HttpGet("GetPotentialLocationUsers/{locationId}")]
        public IActionResult GetPotentialLocationUsers([FromRoute] int locationId)
        {
            return Executor(() => _userRepository.GetPotentialUsersForLocation(Requester, locationId));
        }

        [HttpPut("Disable/{userId}")]
        public async Task<IActionResult> DisableUser([FromRoute] int userId)
        {
            return await Executor(async () => await _userRepository.Disable(Requester, userId));
        }

        [HttpPut("Enable/{userId}")]
        public async Task<IActionResult> Enable([FromRoute] int userId)
        {
            return await Executor(async () => await _userRepository.Enable(Requester, userId));
        }

        // PROJECT

        [HttpGet("GetOfProjectAvailableOnDate/{projectId}/{date}")]
        public IActionResult GetOfProjectAvailableOnDate([FromRoute] int projectId, [FromRoute] DateTime date)
        {
            return Executor(() => _userRepository.GetOfProjectAvailableOnDate(Requester, projectId, date));
        }

        [HttpGet("GetOfProject/{projectId}")]
        public IActionResult GetOfProject([FromRoute] int projectId)
        {
            return Executor(() => _userRepository.GetOfProject(Requester, projectId));
        }

        [HttpGet("GetOfNotProject/{projectId}")]
        public IActionResult GetOfNotProject([FromRoute] int projectId)
        {
            return Executor(() => _userRepository.GetOfNotProject(Requester, projectId));
        }

        [HttpPut("AddUsersToProject/{projectId}")]
        public Task<IActionResult> AddUsers(int projectId, [FromBody] ICollection<int> userIds)
        => Executor(() => _userRepository.AddUsersToProject(Requester, projectId, userIds));

        [HttpPut("RemoveUsersFromProject/{projectId}")]
        public Task<IActionResult> RemoveUsers(int projectId, [FromBody] ICollection<int> userIds)
        => Executor(() => _userRepository.RemoveUsersFromProject(Requester, projectId, userIds));


        [HttpGet("GetInvitedUsers/{workId}")]
        public IActionResult GetInvitedUsers([FromRoute] int workId)
        => Executor(() => _userRepository.GetUsersInvitedToWork(Requester, workId));
    }
}