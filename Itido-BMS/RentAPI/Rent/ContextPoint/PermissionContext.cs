using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class PermissionContext : Context<Permission>
    {
        public PermissionContext(RentContext rentContext) : base(rentContext) { }

        protected override string Permission() => "Permission";

        protected override DbSet<Permission> GetDb() => RentContext.Permission;
    }
}