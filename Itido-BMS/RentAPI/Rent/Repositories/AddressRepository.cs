using System;
using System.Linq;
using System.Threading.Tasks;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models.Projects;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class AddressRepository
    {
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleRepo;

        public AddressRepository(ProjectRoleRepository projectRoleRepository, RentContext rentContext, IRoleAuthenticationRepository roleRepo)
        {
            _projectRoleRepository = projectRoleRepository;
            _context = rentContext;
            _roleRepo = roleRepo;
        }

        public dynamic Get(int requester, int addressId)
        {
            var address = _projectRoleRepository.GetReadableAddresssOfUser(requester).Where(p => p.ID == addressId).Select(Address.BasicDTO()).FirstOrDefault();
            if (address == null) throw new NotFoundException();
            return address;
        }

        public async Task Update(int requester, int addressId, Address address)
        {
            var addressToUpdate = _projectRoleRepository.GetWritableAddresssOfUser(requester).FirstOrDefault(a => a.ID == addressId);
            if (addressToUpdate != null)
            {
                addressToUpdate.AddressName = address.AddressName;
                addressToUpdate.Lat = address.Lat;
                addressToUpdate.Lon = address.Lon;

                _context.Address.Update(addressToUpdate);
                await _context.SaveChangesAsync();
            }
            else
                throw new NotFoundException();
        }
    }
}
