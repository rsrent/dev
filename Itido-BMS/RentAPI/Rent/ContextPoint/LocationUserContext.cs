using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class LocationUserContext : Context<LocationUser>
    {
        public LocationUserContext(RentContext rentContext) : base(rentContext) { }
        
        protected override string Permission() => "Team";
        
        protected override DbSet<LocationUser> GetDb() => RentContext.LocationUser;
        
        protected override IQueryable<LocationUser> SpecialGetRequirement(int requester, IQueryable<LocationUser> query)
        {
            var canViewDisabled = !Unauthorized(requester, Permission(), CRUDD.Delete);
            return query.Where(lu => lu.User.RoleID != 1 && (!lu.User.Disabled || !lu.Location.Disabled || canViewDisabled));
        }
    }
}
