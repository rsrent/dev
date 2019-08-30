using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Role")]
    public class RoleController : Controller
    {
        private string PermissionPermission = "Role";

        private readonly RentContext _context;
        private readonly PermissionRepository _permissionRepository;
        private readonly NotificationRepository _notificationRepository;

        public RoleController(RentContext context, NotificationRepository notificationRepository, PermissionRepository permissionRepository)
        {
            _context = context;
            _permissionRepository = permissionRepository;
            _notificationRepository = notificationRepository;
        }


        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetRoles()
        {
            /*
            if (_permissionRepository.Unauthorized(User, PermissionPermission, CRUDD.Read))
            {
                return Unauthorized();
            } */
            var roles = _context.Role.ToList();
            return Ok(roles);
        }

        [HttpPost("AddRole")]
        [Authorize]
        public async Task<IActionResult> AddRole([FromBody] Role role)
        {
            if (_permissionRepository.Unauthorized(User, PermissionPermission, CRUDD.Create))
            {
                return Unauthorized();
            }

            var anyWithName = _context.Role.Any(r => r.Name == role.Name);
            if(anyWithName) 
            {
                return BadRequest("User with name already exists");
            }

            //var role = new Role { Name = roleName, Rank = 0 };
            _context.Role.Add(role);
            await _context.SaveChangesAsync();

            var templatePermissions = _context.Permission.Select(p => new PermissionsTemplate { PermissionID = p.ID, RoleID = role.ID, Create = false, Update = false, Delete = false, Read = false });
            _context.PermissionsTemplate.AddRange(templatePermissions);
            _context.SaveChanges();

            return Ok();
        }

        [HttpPut("UpdateUserRole/{userID}/{roleID}")]
        [Authorize]
        public async Task<IActionResult> UpdateUserRole([FromRoute] int userID, [FromRoute] int roleID)
        {
            if (_permissionRepository.Unauthorized(User, PermissionPermission, CRUDD.Update))
            {
                return Unauthorized();
            }
            var user = _context.User.Find(userID);
            var role = _context.Role.Find(roleID);

            if(user == null || role == null) 
            {
                return NotFound();
            }

            user.RoleID = roleID;
            _context.User.Update(user);
            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
