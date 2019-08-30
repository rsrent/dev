/*using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Permissions")]
    public class PermissionsController : Controller
    {
        private readonly RentContext _context;

        public PermissionsController(RentContext context)
        {
            _context = context;
        }

        // GET: api/Permissions/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPermissions([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var user = await _context.User.FindAsync(id);
            if(user == null)
            {
                return NotFound("No user with that ID was found");
            }
            var roles = _context.PermissionRole
                .Where(r => r.RoleID == user.RoleID)
                .Select(p => new PermissionGetDTO
                {
                    CRUD = p.Permission.CRUD,
                    DisplayName = p.Permission.DisplayName,
                    SystemName = p.Permission.SystemName,
                    ID = p.Permission.ID
                });
            return Ok(roles);
        }

        // PUT: api/Permissions/Role/5/Permission/2
        [HttpPut("Role/{roleId}/Permission/{permissionId}")]
        public async Task<IActionResult> AddPermissionToRole([FromRoute] int roleId, [FromRoute] int permissionId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _context.Role.Find(roleId);
            var permission = _context.Permission.Find(permissionId);
            if(role == null)
            {
                return NotFound("A role with that ID was not found.");
            }
            if (permission == null)
            {
                return NotFound("A permission with that ID was not found.");
            }
            await _context.PermissionRole.AddAsync(new Models.PermissionRole
            {
                RoleID = roleId,
                PermissionID = permissionId
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // POST: api/Permissions/Role/5/Permission/2
        [HttpPost("AddPermission")]
        public async Task<IActionResult> AddPermission([FromBody] PermissionAddDTO permissionAddDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var role = _context.Role.Find(permissionAddDTO.RoleID);
            if (role == null)
            {
                return NotFound("A role with that ID was not found. " +
                    "It is not possible to add a permission without assigning it to a role.");
            }
            var permission = new Models.Permission
            {
                DisplayName = permissionAddDTO.DisplayName,
                CRUD = permissionAddDTO.CRUD,
                SystemName = permissionAddDTO.SystemName
            };
            await _context.Permission.AddAsync(permission);
            await _context.SaveChangesAsync();
            await _context.PermissionRole.AddAsync(new Models.PermissionRole
            {
                RoleID = permissionAddDTO.RoleID,
                PermissionID = permission.ID
            });
            await _context.SaveChangesAsync();
            return NoContent();
        }



    }
}
*/