using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Route("api/[controller]")]
    //[AllowAnonymous]
    public class LocationController : ControllerExecutor
    {
        //private readonly RentContext _context;
        //private readonly PermissionRepository _permissionRepository;
        private readonly LocationRepository _locationRepository;

        public LocationController(LocationRepository locationRepository)
        {
            _locationRepository = locationRepository;

        }

        //private int Requester = 1;

        [HttpGet("GetName/{locationId}")]
        public IActionResult GetName([FromRoute] int locationId)
        => Executor(() => _locationRepository.GetName(Requester, locationId));

        [HttpGet]
        public IActionResult GetAll()
        => Executor(() => _locationRepository.GetAll(Requester));

        [HttpGet("{locationId}")]
        public IActionResult GetInformationAboutLocation(int locationId)
        => Executor(() => _locationRepository.GetLocationInformation(Requester, locationId));

        [HttpDelete("{locationId}")]
        public async Task<IActionResult> Disable(int locationId)
        => await Executor(async () => await _locationRepository.Disable(Requester, locationId));

        [HttpPut("Enable/{locationId}")]
        public async Task<IActionResult> Enable(int locationId)
        => await Executor(async () => await _locationRepository.Enable(Requester, locationId));

        [HttpGet("GetForUser/{userId}")]
        public IActionResult GetForUser([FromRoute] int userId)
        => Executor(() => _locationRepository.GetLocationsForUser(Requester, userId));

        [HttpGet("GetForCustomer/{customerId}")]
        public IActionResult GetForCustomer(int customerId)
        => Executor(() => _locationRepository.GetLocationsForCustomer(Requester, customerId));

        [HttpPost("AddLocation/{customerId}")]
        public async Task<IActionResult> AddLocation(int customerId, [FromBody] Location location)
        => await Executor(async () => await _locationRepository.Add(Requester, customerId, location));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Location updatedLocation)
        => await Executor(async () => await _locationRepository.Update(Requester, updatedLocation));

        [HttpPut("AddUser/{locationId}/{userId}")]
        public async Task<IActionResult> AddUser([FromRoute] int locationId, [FromRoute] int userId)
        => await Executor(async () => await _locationRepository.AddUserToLocation(Requester, locationId, userId));

        [HttpPut("AddUserToLocations/{userId}")]
        public async Task<IActionResult> AddUserToLocations([FromRoute] int userId, [FromBody] ICollection<int> locationIds)
        => await Executor(async () => await _locationRepository.AddUserToLocations(Requester, locationIds, userId));

        [HttpPut("UpdateLocationUserTitle/{locationId}/{userId}/{title}")]
        public async Task<IActionResult> UpdateLocationUserTitle([FromRoute] int locationId, [FromRoute] int userId, [FromRoute] string title)
        => await Executor(async () => await _locationRepository.UpdateLocationUser(Requester, locationId, userId, title, null));

        [HttpPut("UpdateLocationUserHourtext/{locationId}/{userId}/{hourText}")]
        public async Task<IActionResult> UpdateLocationUserHourtext([FromRoute] int locationId, [FromRoute] int userId, [FromRoute] string hourText)
        => await Executor(async () => await _locationRepository.UpdateLocationUser(Requester, locationId, userId, null, hourText));

        [HttpGet("GetUsersLocations/{userId}")]
        public IActionResult GetUsersLocations([FromRoute] int userId)
        => Executor(() => _locationRepository.GetUsersLocations(Requester, userId));

        [HttpGet("GetNotUsersLocations/{userId}")]
        public IActionResult GetNotUsersLocations([FromRoute] int userId)
        => Executor(() => _locationRepository.GetNotUsersLocations(Requester, userId));

        [HttpPut("RemoveUser/{locationId}/{userId}")]
        public async Task<IActionResult> RemoveUser([FromRoute] int locationId, [FromRoute] int userId)
        => await Executor(async () => await _locationRepository.RemoveUserFromLocations(Requester, new[] { locationId }, userId));

        [HttpPut("RemoveUserFromLocations/{userId}")]
        public async Task<IActionResult> RemoveUserFromLocations([FromRoute] int userId, [FromBody] ICollection<int> locationIds)
        => await Executor(async () => await _locationRepository.RemoveUserFromLocations(Requester, locationIds, userId));
    }
}