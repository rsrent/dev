using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.Models.Projects;
using Rent.Models.TimePlanning;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/ProjectRole")]
    public class ProjectRoleController : ControllerExecutor
    {
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly RentContext _context;
        public ProjectRoleController(ProjectRoleRepository projectRoleRepository, RentContext context)
        {
            _projectRoleRepository = projectRoleRepository;
            _context = context;
        }

        [HttpGet("GetRoles")]
        public IActionResult GetRoles()
        {
            return Ok(_context.ProjectRole.ToList());
        }
    }
}
