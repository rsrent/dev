using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;
using API.Data;
using API.Exceptions;
using System.Linq;

namespace API.Repositories
{
    public class UserRepository
    {
        private readonly BMSContext _context;

        public UserRepository(BMSContext context)
        {
            this._context = context;
        }

        public User GetFromId(long Id)
        {
            return _context.Users.FirstOrDefault(u => u.Id == Id);
        }

        public ICollection<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public async Task UpdateAsMaster(long id, User val)
        {
            var toUpdate = await _context.Users.FindAsync(id);
            if (toUpdate == null) throw new NotFoundException();
            // update values
            toUpdate.Name = val.Name;
            toUpdate.Email = val.Email;
            // commit changes
            _context.Users.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Create(User client)
        {
            var res = _context.Users.Add(client);
            await _context.SaveChangesAsync();
            return client.Id;
        }

        public async Task Delete(long id)
        {
            var toDelete = _context.Users.FirstOrDefault(c => c.Id == id);
            _context.Users.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
