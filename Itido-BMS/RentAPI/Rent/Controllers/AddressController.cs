using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.Projects;
using Rent.Repositories;
namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Address")]
    public class AddressController : ControllerExecutor
    {
        private readonly AddressRepository _addressRepository;
        public AddressController(AddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        [HttpGet("Get/{addressId}")]
        public IActionResult Get(int addressId)
        => Executor(() => _addressRepository.Get(Requester, addressId));

        [HttpPut("Update/{addressId}")]
        public Task<IActionResult> Update(int addressId, [FromBody] Address address)
        => Executor(() => _addressRepository.Update(Requester, addressId, address));
    }
}
