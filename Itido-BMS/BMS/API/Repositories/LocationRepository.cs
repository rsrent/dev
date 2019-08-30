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
    public class LocationRepository
    {
        private readonly BMSContext _context;

        public LocationRepository(BMSContext context)
        {
            this._context = context;
        }

        public Location GetFromId(long Id)
        {
            return _context.Locations.FirstOrDefault(u => u.Id == Id);
        }

        public ICollection<Location> GetAll()
        {
            return _context.Locations.ToList();
        }

        public async Task UpdateAsMaster(long id, Location val)
        {
            var toUpdate = await _context.Locations.FindAsync(id);
            if (toUpdate == null) throw new NotFoundException();
            // update values
            toUpdate.Name = val.Name;
            // commit changes
            _context.Locations.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Create(Location client)
        {
            var res = _context.Locations.Add(client);
            await _context.SaveChangesAsync();
            return client.Id;
        }

        public async Task Delete(long id)
        {
            var toDelete = _context.Locations.FirstOrDefault(c => c.Id == id);
            _context.Locations.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
