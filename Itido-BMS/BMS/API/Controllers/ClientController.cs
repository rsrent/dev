using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using API.Repositories;
using API.Models;
using API.Data;

//ADD user to client needs to be done by admin

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ClientController : ControllerExecutor
    {
        protected readonly ClientRepository repository;

        public ClientController(ClientRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Executor(() => repository.GetAll());
        }

        [Authorize]
        [HttpGet("getForClient/{id}")]
        public IActionResult Get(long id)
        {
            return Executor(() => repository.GetFromId(id));
        }

        [Authorize(Roles = "Master,Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> Post([FromBody]Client value)
        {
            return await Executor(() => repository.Create(value));
        }

        [Authorize(Roles = "Master,Admin")]
        [HttpPut("update/{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]Client value)
        {
            return await Executor(() => repository.UpdateAsMaster(id, value));
        }

        [Authorize(Roles = "Master,Admin")]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Executor(() => repository.Delete(id));
        }
    }
}
