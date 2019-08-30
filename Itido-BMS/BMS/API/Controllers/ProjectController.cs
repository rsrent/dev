﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Repositories;
using API.Models;
using API.Data;

namespace API.Controllers
{
    [Route("api/[controller]")]
    public class ProjectController : ControllerExecutor
    {

        protected readonly ProjectRepository repository;

        public ProjectController(ProjectRepository repo)
        {
            this.repository = repo;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Executor(() => repository.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            return Executor(() => repository.GetFromId(id));
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Project value)
        {
            return await Executor(() => repository.Create(value));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(long id, [FromBody]Project value)
        {
            return await Executor(() => repository.UpdateAsMaster(id, value));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return await Executor(() => repository.Delete(id));
        }
    }
}
