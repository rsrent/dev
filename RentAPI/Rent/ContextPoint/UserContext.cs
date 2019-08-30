using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint.Exceptions;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class UserContext : ContextDto<User>
    {
        public UserContext(RentContext rentContext, PropCondition condition) : base(rentContext, condition) {}

        protected override string Permission() => "User";
        
        protected override DbSet<User> GetDb() => RentContext.User;
        
        protected override IQueryable<User> SpecialGetRequirement(int requester, IQueryable<User> query)
        {
            var canViewDisabled = !Unauthorized(requester, Permission(), CRUDD.Delete);
            return query.Where(u => u.RoleID != 1 && (!u.Disabled || canViewDisabled));
        }

        public override ContextRules GetRules() => new Rules();

        public class Rules : ContextRules
        {
            public override string ThisKey(string key = null) => key?? "User";
            
            public Rules() { }
        
            public override Dictionary<string, List<string>> GetUnallowed(PropCondition condition, string key = null)
            {
                var unallowed = new Dictionary<string, List<string>> {{ThisKey(key), new List<string>()}};
                
                if (condition.IsCustomer())
                {
                    unallowed[ThisKey(key)].Add("Comment");
                }

                return unallowed;
            }
        }
    }
}