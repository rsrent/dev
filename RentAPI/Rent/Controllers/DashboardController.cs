using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Models;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Dashboard")]
    [AllowAnonymous]
    public class DashboardController : ControllerExecutor
    {
        private readonly DashboardRepository _dashboardRepository;

        public DashboardController(DashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        private int Requester = 1;

        [HttpGet("Data/{customerId}/{userId}/{locationId}")]
        public IActionResult Data([FromRoute] int customerId, [FromRoute] int userId, [FromRoute] int locationId)
        => Executor(() => _dashboardRepository.Data(Requester, customerId, userId, locationId));
        
        [HttpGet("Customers")]
        public IActionResult Customers()
            => Executor(() => _dashboardRepository.Customers(Requester));

        [HttpGet("Users")]
        public IActionResult Users()
            => Executor(() => _dashboardRepository.Users(Requester));
        
        [HttpGet("Locations/{customerId}/{userId}")]
        public IActionResult Locations([FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.Locations(Requester, customerId, userId));
        
        [HttpGet("LocationsWithoutPlan/{customerId}/{userId}")]
        public IActionResult LocationsWithoutPlan([FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.LocationsWithoutPlan(Requester, customerId, userId));
        
        [HttpGet("LocationsWithoutStaff/{customerId}/{userId}")]
        public IActionResult LocationsWithoutStaff([FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.LocationsWithoutStaff(Requester, customerId, userId));
        
        [HttpGet("LocationsWithoutServiceLeader/{customerId}")]
        public IActionResult LocationsWithoutServiceLeader([FromRoute] int customerId)
            => Executor(() => _dashboardRepository.LocationsWithoutServiceLeader(Requester, customerId));
        
        [HttpGet("Tasks/{locationId}/{customerId}/{userId}")]
        public IActionResult Tasks([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.Tasks(Requester, locationId, customerId, userId));
        
        [HttpGet("Logs/{locationId}/{customerId}/{userId}")]
        public IActionResult Logs([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.Logs(Requester, locationId, customerId, userId));
        
        [HttpGet("MoreWork/{locationId}/{customerId}/{userId}")]
        public IActionResult MoreWork([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.MoreWork(Requester, locationId, customerId, userId));

        [HttpGet("News/{locationId}/{customerId}/{userId}")]
        public IActionResult News([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.News(Requester, locationId, customerId, userId));

        [HttpGet("Dg/{locationId}/{customerId}/{userId}")]
        public IActionResult Dg([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId)
            => Executor(() => _dashboardRepository.GetDg(Requester, locationId, customerId, userId));

        [HttpGet("WorkHistory/{locationId}/{customerId}/{userId}/{from}/{daysBack}")]
        public IActionResult Dg([FromRoute] int locationId, [FromRoute] int customerId, [FromRoute] int userId, [FromRoute] DateTime from, [FromRoute] int daysBack)
        => Executor(() => _dashboardRepository.WorkHistory(Requester, locationId, customerId, userId, daysBack, from));
        
        [HttpGet("Holidays")]
        public IActionResult Holidays()
            => Executor(() => _dashboardRepository.getHolidays());
        
        /*
        
        [HttpGet("Data/Customer/{id}")]
        public IActionResult CustomerData([FromRoute] int id)
            => Executor(() => _dashboardRepository.CustomerData(Requester, id));
        
        [HttpGet("Data/ServiceLeader/{id}")]
        public IActionResult ServiceLeaderData([FromRoute] int id)
            => Executor(() => _dashboardRepository.ServiceLeaderData(Requester, id));

        [HttpGet("Logs")]
        public IActionResult Logs()
        => Executor(() => _dashboardRepository.Logs(Requester));

        [HttpGet("Tasks")]
        public IActionResult Tasks()
        => Executor(() => _dashboardRepository.Tasks(Requester));

        [HttpGet("Tasks/Location/{id}")]
        public IActionResult LocationTasks([FromRoute] int id)
        => Executor(() => _dashboardRepository.LocationTasks(Requester, id));

        [HttpGet("Locations")]
        public IActionResult Locations()
        => Executor(() => _dashboardRepository.Locations(Requester));

        [HttpGet("Locations/Customer/{id}")]
        public IActionResult CustomerLocations([FromRoute] int id)
        => Executor(() => _dashboardRepository.CustomerLocations(Requester, id));

        [HttpGet("Locations/ServiceLeader/{id}")]
        public IActionResult ServiceLeaderLocations([FromRoute] int id)
        => Executor(() => _dashboardRepository.ServiceLeaderLocations(Requester, id));
        
        [HttpGet("Customers")]
        public IActionResult Customers()
        => Executor(() => _dashboardRepository.Customers(Requester));

        [HttpGet("Users")]
        public IActionResult Users()
        => Executor(() => _dashboardRepository.Users(Requester));

        
        
        
        [AllowAnonymous]
        [HttpGet("LocationsNoPlan")]
        public IActionResult LocationsNoPlan()
            => Executor(() => _dashboardRepository.LocationsWithoutPlan(1));
        
        [HttpGet("LocationsNoPlan/Customer/{id}")]
        public IActionResult CustomerLocationsNoPlan([FromRoute] int id)
            => Executor(() => _dashboardRepository.CustomerLocationsWithoutPlan(Requester, id));
        
        
        
        [AllowAnonymous]
        [HttpGet("LocationsNoStaff")]
        public IActionResult LocationsNoStaff()
            => Executor(() => _dashboardRepository.LocationsWithoutStaff(1));
        
        [HttpGet("LocationsNoStaff/Customer/{id}")]
        public IActionResult CustomerLocationsNoStaff([FromRoute] int id)
            => Executor(() => _dashboardRepository.CustomerLocationsWithoutStaff(Requester, id));
        
        [HttpGet("LocationsNoStaff/ServiceLeader/{id}")]
        public IActionResult ServiceLeaderLocationsNoStaff([FromRoute] int id)
            => Executor(() => _dashboardRepository.Ser(Requester, id));
        
        
        [AllowAnonymous]
        [HttpGet("RentTest")]
        public IActionResult RentTest()
            => Executor(() => _dashboardRepository.Data(1));
        
        [AllowAnonymous]
        [HttpGet("LocationsTest")]
        public IActionResult LocationsTest()
            => Executor(() => _dashboardRepository.Locations(1));
        
        [AllowAnonymous]
        [HttpGet("RentTestOld")]
        public IActionResult RentTestOld()
            => Executor(() => _dashboardRepository.Data(1));
        
        [AllowAnonymous]
        [HttpGet("UsersTest")]
        public IActionResult UsersTest()
            => Executor(() => _dashboardRepository.Users(1));
        
        [AllowAnonymous]
        [HttpGet("CustomersTest")]
        public IActionResult CustomersTest()
            => Executor(() => _dashboardRepository.Customers(1));
            */
    }
}