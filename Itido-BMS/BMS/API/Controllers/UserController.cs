using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.DTOs;
using API.Models;
using API.Data;
// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class UserController : ControllerExecutor
    {
        protected readonly UserRepository repository;

        public UserController(UserRepository repo)
        {
            this.repository = repo;
        }

        // GET: api/values
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Executor(() => repository.GetAll());
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            return Executor(() => repository.GetFromId(id));
        }

        // POST api/values
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            return await Executor(() => repository.Create(user));
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] User user)
        {
            return await Executor(() => repository.UpdateAsMaster(id, user));
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Executor(() => repository.Delete(id));
        }
    }
}
