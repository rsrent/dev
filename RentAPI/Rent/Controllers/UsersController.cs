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

        [HttpPut]
        public async Task<IActionResult> GetUser([FromRoute] User user)
        {
            return await Executor(async () => await _userRepository.UpdateUser(Requester, user));
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

        [HttpDelete("{userId}")]
        public async Task<IActionResult> DisableUser([FromRoute] int userId)
        {
            return await Executor(async () => await _userRepository.Disable(Requester, userId));
        }

        [HttpPut("Enable/{userId}")]
        public async Task<IActionResult> Enable([FromRoute] int userId)
        {
            return await Executor(async () => await _userRepository.Enable(Requester, userId));
        }
    }
}