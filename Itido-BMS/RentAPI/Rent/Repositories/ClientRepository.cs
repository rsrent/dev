using System;
using System.Linq;
using Rent.Data;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;

namespace Rent.Repositories
{
    public class ClientRepository
    {
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleRepo;
        public ClientRepository(RentContext context, IRoleAuthenticationRepository roleRepo)
        {
            _context = context;
            _roleRepo = roleRepo;
        }

        public IQueryable<dynamic> GetClients(int requester)
        {
            if (_roleRepo.IsAdminOrManager(requester))
            {
                return _context.Client.Select(Client.StandardDTO());
            }
            throw new UnauthorizedAccessException();
        }
    }
}
