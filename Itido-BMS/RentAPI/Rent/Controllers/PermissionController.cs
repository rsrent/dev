using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Permission")]
    public class PermissionController : ControllerExecutor
    {
        private readonly PermissionRepository _permissionRepository;

        public PermissionController(PermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        
        [HttpGet]
        public IActionResult Get()
        => Executor(() => _permissionRepository.GetPermissions(Requester));

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] Permission permission)
        => await Executor(async () => await _permissionRepository.CreatePermission(Requester, permission));

        [HttpGet("GetUserPermissions/{userID}")]
        public IActionResult GetUserPermissions([FromRoute] int userId)
        => Executor(() => _permissionRepository.GetUserPermissions(Requester, userId));

        [HttpPut("UpdateUserPermission/{userID}/{PermissionID}/{crudd}/{active}")]
        public async Task<IActionResult> UpdateUserPermission([FromRoute] int userId, [FromRoute] int permissionId, [FromRoute] CRUDD crudd, [FromRoute] bool active)
        => await Executor(async () => await _permissionRepository.UpdateUserPermission(Requester, userId, permissionId, crudd, active));

        [HttpGet("GetPermissionTemplates/{roleID}")]
        public IActionResult GetPermissionTemplates([FromRoute] int roleId)
        => Executor(() => _permissionRepository.GetTemplatePermissions(Requester, roleId));

        [HttpPut("UpdatePermissionTemplate/{roleID}/{permissionID}/{crudd}/{active}")]
        public async Task<IActionResult> UpdatePermissionTemplate([FromRoute] int roleId, [FromRoute] int permissionId, [FromRoute] CRUDD crudd, [FromRoute] bool active)
        => await Executor(async () => await _permissionRepository.UpdateTemplatePermission(Requester, roleId, permissionId, crudd, active));

        [HttpPut("ResetUserPermissions/{userID}")]
        public async Task<IActionResult> ResetUserPermissions([FromRoute] int userId) 
        => await Executor(async () => await _permissionRepository.ResetUserPermissions(Requester ,userId));

        [HttpPut("ResetUsersSpecificPermissions/{permissionID}")]
        public async Task<IActionResult> ResetUsersSpecificPermissions([FromRoute] int permissionId)
        => await Executor(async () => await _permissionRepository.ResetUsersSpecificPermissions(Requester ,permissionId));
    }
}
