using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class UserPermissionContext : Context<UserPermissions>
    {
        public UserPermissionContext(RentContext rentContext) : base(rentContext) { }

        protected override string Permission() => "Permission";
        
        protected override DbSet<UserPermissions> GetDb() => RentContext.UserPermissions;

        protected override IQueryable<UserPermissions> SpecialUpdateRequirement(int requester, IQueryable<UserPermissions> query)
        {
            var user = RentContext.User.Find(requester);
            return query.Where(up => !up.Permission.OnlyMasterCanChange || user.RoleID == 1);
        }
    }
}