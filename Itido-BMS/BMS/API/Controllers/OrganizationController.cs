using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Repositories;
using API.Models;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class OrganizationController : ControllerExecutor
    {
        protected readonly OrganizationRepository repository;

        public OrganizationController(OrganizationRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Executor(() => repository.GetAll());
        }

        [Authorize]
        [HttpGet("getForOrganization/{id}")]
        public IActionResult Get(long id)
        {
            return Executor(() => repository.GetFromId(id));
        }

        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody]Organization value)
        {
            return await Executor(() => repository.Create(value));
        }

        [Authorize]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]Organization value)
        {
            return await Executor(() => repository.UpdateAsMaster(id, value));
        }

        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Executor(() => repository.Delete(id));
        }
    }
}
