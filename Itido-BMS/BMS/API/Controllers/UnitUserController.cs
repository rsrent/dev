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
    public class UnitUserController : ControllerExecutor
    {
        protected readonly UnitUserRepository repository;


        public UnitUserController(UnitUserRepository repo)
        {
            this.repository = repo;
        }

        [Authorize(Roles = "Master,Admin, Manager")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]UnitUser value)
        {
            return await Executor(() => repository.CreateUnitUser(value));
        }

        

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int UnitId, int UserId)
        {
            return await Executor(() => repository.DeleteUnitUser(UnitId, UserId));
        }

    }

}