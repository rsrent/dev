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
    [Route("api/Client")]
    public class ClientController : ControllerExecutor
    {
        private readonly ClientRepository _clientRepository;
        public ClientController(ClientRepository clientRepository)
        {
            _clientRepository = clientRepository;
        }

        [HttpGet("GetClients")]
        public IActionResult GetClients()
        => Executor(() => _clientRepository.GetClients(Requester));
    }
}
