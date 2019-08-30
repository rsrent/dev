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
    public class TemplatePermissionContext : Context<PermissionsTemplate>
    {
        public TemplatePermissionContext(RentContext rentContext) : base(rentContext) { }

        protected override string Permission() => "Permission";
        
        protected override DbSet<PermissionsTemplate> GetDb() => RentContext.PermissionsTemplate;
        
        protected override IQueryable<PermissionsTemplate> SpecialUpdateRequirement(int requester, IQueryable<PermissionsTemplate> query)
        {
            var user = RentContext.User.Find(requester);
            return query.Where(tp => (!tp.Permission.OnlyMasterCanChange && tp.RoleID != 1) || user.RoleID == 1);
        }
    }
}