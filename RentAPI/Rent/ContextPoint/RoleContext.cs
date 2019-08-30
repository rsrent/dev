using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class RoleContext : Context<Role>
    {
        public RoleContext(RentContext rentContext) : base(rentContext) { }

        protected override string Permission() => "Role";

        protected override DbSet<Role> GetDb() => RentContext.Role;
    }
}