//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Rent.Data;
//using Rent.DTOs;
//using Rent.Models;
//using Rent.Repositories;

//// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

//namespace Rent.Controllers
//{
    
//    [Produces("application/json")]
//    [Route("api/Admin")]
//    public class AdminController : Controller
//    {
//        private readonly RentContext _context;
//        private readonly LocationRepository _locationRepository;

//        public AdminController(RentContext context, LocationRepository locationRepository)
//        {
//            _context = context;
//            _locationRepository = locationRepository;
//        }
//        //*
//        //USERS
//        [AllowAnonymous]
//        [HttpGet("Users")]
//        public async Task<IActionResult> GetUsers()
//        {
//            var users =
//                await _context.User
//                        .Include(u => u.Role)
//                        .Include(u => u.Customer)
//                        .Where(u => u.RoleID != 1)
//                        .Select(u => new UserDTO(u)).ToListAsync();
//            return Ok(users);
//        }

//        [AllowAnonymous]
//        [HttpPost("Users")]
//        public async Task<IActionResult> CreateUsers([FromBody] UserLoginCreateDTO userLoginCreateDTO)
//        {
//            try {
//                if (!ModelState.IsValid)
//                {
//                    return BadRequest(ModelState);
//                }
//                if (userLoginCreateDTO.User.Role.ID == 1 || userLoginCreateDTO.User.RoleID == 1)
//                {
//                    return BadRequest("Master make you...");
//                }

//                (bool, string, User) result = await new LoginRepository(_context).AddLogin(userLoginCreateDTO);
//                if (!result.Item1)
//                {
//                    return BadRequest(result.Item2);
//                }
//                else
//                {
//                    return Ok(result.Item3);
//                }
//            } catch (Exception exc)
//            {
//                return BadRequest(exc.Message);
//            }
//        }

//        [AllowAnonymous]
//        [HttpPut("Users")]
//        public async Task<IActionResult> UpdateUsers([FromBody] User updatedUser)
//        {
//            var user = _context.User.Include(u => u.Role).FirstOrDefault(u => u.ID == updatedUser.ID);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            user.FirstName = updatedUser.FirstName;
//            user.LastName = updatedUser.LastName;
//            user.Email = updatedUser.Email;
//            user.Comment = updatedUser.Comment;
//            user.ImageLocation = updatedUser.ImageLocation;
//            user.EmployeeNumber = updatedUser.EmployeeNumber;
//            user.Title = updatedUser.Title;
//            user.Phone = updatedUser.Phone;
 
//            _context.User.Update(user);
//            await _context.SaveChangesAsync();
//            return Ok(user);
//        }

//        /*
//        [HttpDelete("Users")]
//        public async Task<IActionResult> DeleteUsers([FromRoute] int userID)
//        {
//            var user = await _context.User.FindAsync(userID);
//            if (user == null)
//                return BadRequest("User not found");
//            _context.User.Remove(user);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }
//        */

//        //LOCATIONS
//        [AllowAnonymous]
//        [HttpGet("Locations")]
//        public async Task<IActionResult> GetLocations()
//        {
//            var locations = 
//                await _context
//                    .Location
//                    .Include(l => l.Customer)
//                    .Include(l => l.ServiceLeader)
//                    .Include(l => l.CustomerContact)
//                    .OrderBy(l => l.Name)
//                    .Select(l => new LocationDTO(l)).ToListAsync();
//            return Ok(locations);
//        }

//        /*
//        [HttpPost("Locations")]
//        public async Task<IActionResult> CreateLocations([FromBody] Location location)
//        {
//            var newLocation = await _locationRepository.Add(1, (int) location.CustomerID, location);
//            if (newLocation == null)
//            {
//                return StatusCode(500); //Internal server error
//            }
//            return Ok();
//        }

//        [HttpPut("Locations")]
//        public async Task<IActionResult> UpdateLocations([FromBody] Location location)
//        {
//            if (!_context.Location.Any(u => u.ID == location.ID))
//                return BadRequest("Location not found");
//            _context.Location.Update(location);
//            await _context.SaveChangesAsync();
//            return Ok(location);
//        }


//        [HttpDelete("Locations")]
//        public async Task<IActionResult> DeleteLocations([FromRoute] int locationID)
//        {
//            var location = await _context.Location.FindAsync(locationID);
//            if (location == null)
//                return BadRequest("Location not found");
//            _context.Location.Remove(location);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }
//*/

//        //CUSTOMERS
//        [AllowAnonymous]
//        [HttpGet("Customers")]
//        public async Task<IActionResult> GetCustomers()
//        {
//            var customers =
//                await _context
//                    .Customer
//                    .Include(c => c.MainUser)
//                    .Include(c => c.KeyAccountManager)
//                    .Select(c => new CustomerDTO(c))
//                    .OrderBy(c => c.Name)
//                    .ToListAsync();
//            return Ok(customers);
//        }

//        /*
//        [HttpPost("Customers")]
//        public async Task<IActionResult> CreateCustomers([FromBody] Customer customer)
//        {
//            await _context.Customer.AddAsync(customer);
//            await _context.SaveChangesAsync();
//            return Ok(customer);
//        }

//        [HttpPut("Customers")]
//        public async Task<IActionResult> UpdateCustomers([FromBody] Customer customer)
//        {
//            if (!_context.Customer.Any(u => u.ID == customer.ID))
//                return BadRequest("Customer not found");
//            _context.Customer.Update(customer);
//            await _context.SaveChangesAsync();
//            return Ok(customer);
//        }


//        [HttpDelete("Customers")]
//        public async Task<IActionResult> DeleteCustomers([FromRoute] int customerID)
//        {
//            var customer = await _context.Customer.FindAsync(customerID);
//            if (customer == null)
//                return BadRequest("Customer not found");
//            _context.Customer.Remove(customer);
//            await _context.SaveChangesAsync();
//            return Ok();
//        }
//*/



//        //OTHER
//        [AllowAnonymous]
//        [HttpGet("Roles")]
//        public async Task<IActionResult> GetRoles()
//        {
//            var roles = _context.Role.ToList();
//            return Ok(roles);
//        }

//        //*/
//    }
//}
