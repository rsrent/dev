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
    public class UnitUserRepository
    {
        private readonly BMSContext _context;

        public UnitUserRepository(BMSContext context)
        {
            this._context = context;
        }

        public async Task<long> CreateUnitUser(UnitUser unitUser)
        {
            var res = _context.UnitUsers.Add(unitUser);
            await _context.SaveChangesAsync();
            return unitUser.UserId;
        }

        public async Task DeleteUnitUser(long UnitId, long UserId)
        {
            var toDelete = _context.UnitUsers.FirstOrDefault(c => c.UserId == UserId && c.UnitId == UnitId);
            _context.UnitUsers.Remove(toDelete);
            await _context.SaveChangesAsync();
        }
        

    }

}