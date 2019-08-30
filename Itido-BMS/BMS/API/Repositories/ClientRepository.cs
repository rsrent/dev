using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Data;
using API.DTOs;
using API.Exceptions;
using System.Linq;

namespace API.Repositories
{
    public class ClientRepository
    {
        private readonly BMSContext _context;

        public ClientRepository(BMSContext context)
        {
            this._context = context;
        }

        public Client GetFromId(long Id)
        {
            return _context.Clients.FirstOrDefault(u => u.Id == Id);
        }

        public ICollection<Client> GetAll()
        {
            return _context.Clients.ToList();
        }

        public async Task UpdateAsMaster(long id, Client val)
        {
            var toUpdate = await _context.Clients.FindAsync(id);
            if (toUpdate == null) throw new NotFoundException();
            // update values
            toUpdate.Name = val.Name;
            // commit changes
            _context.Clients.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Create(Client client)
        {
            var res = _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client.Id;
        }

        public async Task Delete(long id)
        {
            var toDelete = _context.Clients.FirstOrDefault(c => c.Id == id);
            _context.Clients.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
