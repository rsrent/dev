using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent.Models;
using Rent.Data;
using Rent.DTOs;
using Rent.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Rent.EmailTemplates;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Customers")]
    public class CustomersController : ControllerExecutor
    {
        private readonly CustomerRepository _customerRepository;

        public CustomersController(CustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        
        [HttpGet("{id}")]
        public IActionResult Get([FromRoute] int id)
        => Executor(() => _customerRepository.Get(Requester, id));

        [HttpGet]
        public IActionResult GetAll()
        => Executor(() => _customerRepository.GetAll(Requester));

        [HttpGet("GetCustomerName/{id}")]
        public IActionResult GetCustomerName([FromRoute] int id)
        => Executor(() => _customerRepository.CustomerName(Requester, id));

        [HttpGet("GetForCustomerUser/{userId}")]
        public IActionResult GetForCustomerUser([FromRoute]int userId)
        => Executor(() => _customerRepository.GetUsersCustomer(Requester, userId));

        [HttpDelete("{customerId}")]
        public async Task<IActionResult> Disable([FromRoute]int customerId)
        => await Executor(async () => await _customerRepository.Disable(Requester, customerId));

        [HttpPut("Enable/{customerId}")]
        public async Task<IActionResult> Enable([FromRoute]int customerId)
        => await Executor(async () => await _customerRepository.Enable(Requester, customerId));

        [HttpPost("AddCustomer")]
        public async Task<IActionResult> AddCustomer([FromBody] Customer newCustomer)
        => await Executor(async () => await _customerRepository.Create(Requester, newCustomer));  

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] Customer updatedCustomer)
        => await Executor(async () => await _customerRepository.Update(Requester, updatedCustomer));

        [HttpPut("Invite/{customerID}")]
        public async Task<IActionResult> Invite([FromRoute] int customerId)
        => await Executor(async () => await _customerRepository.Invite(Requester, customerId, null));
    }
}