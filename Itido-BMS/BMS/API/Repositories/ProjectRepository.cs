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
    public class ProjectRepository
    {
        private readonly BMSContext _context;

        public ProjectRepository(BMSContext context)
        {
            this._context = context;
        }

        public Project GetFromId(long Id)
        {
            return _context.Projects.FirstOrDefault(u => u.Id == Id);
        }

        public ICollection<Project> GetAll()
        {
            return _context.Projects.ToList();
        }

        public async Task UpdateAsMaster(long id, Project val)
        {
            var toUpdate = await _context.Projects.FindAsync(id);
            if (toUpdate == null) throw new NotFoundException();
            // update values
            toUpdate.Name = val.Name;
            // commit changes
            _context.Projects.Update(toUpdate);
            await _context.SaveChangesAsync();
        }

        public async Task<long> Create(Project client)
        {
            var res = _context.Projects.Add(client);
            await _context.SaveChangesAsync();
            return client.Id;
        }

        public async Task Delete(long id)
        {
            var toDelete = _context.Projects.FirstOrDefault(c => c.Id == id);
            _context.Projects.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
    }
}
