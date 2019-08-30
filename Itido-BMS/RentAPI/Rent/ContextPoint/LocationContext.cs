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
    public class LocationContext : ContextDto<Location>
    {
        private readonly UserContext.Rules _userRules;

        public LocationContext(RentContext rentContext, PropCondition condition, UserContext.Rules userRules) : base(
            rentContext, condition)
        {
            _userRules = userRules;
        }

        protected override string Permission() => "Location";
        
        protected override DbSet<Location> GetDb() => RentContext.Location;
        
        protected override IQueryable<Location> SpecialGetRequirement(int requester, IQueryable<Location> query)
        {
            var canViewDisabled = !Unauthorized(requester, Permission(), CRUDD.Delete);
            query = query.Where(u => (!u.Disabled || canViewDisabled));

            if (requester != 0)
            {
                var user = RentContext.User.Find(requester);
                if (user.CustomerID != null)
                    query = query.Where(c => c.CustomerID == user.CustomerID);
                if(user.RoleID == 8)
                {
                    query = query.Where(l => l.LocationUsers.Any(lu => lu.UserID == requester));
                }
            }
            
            return query;
        }
        
        public override ContextRules GetRules() => new Rules(_userRules);
        
        public class Rules : ContextRules
        {
            public override string ThisKey(string key = null) => key?? "Location";
            
            private readonly UserContext.Rules _userRules;

            public Rules() { }
            
            internal Rules(UserContext.Rules userRules)
            {
                _userRules = userRules;
            }
        
            public override Dictionary<string, List<string>> GetUnallowed(PropCondition condition, string key = null)
            {
                var unallowed = new Dictionary<string, List<string>> {{ThisKey(key), new List<string>()}};
                
                if (condition.IsCustomer())
                {
                    unallowed[ThisKey(key)].Add("Comment");
                }

                if (!condition.HasReadPermission("Economy"))
                {
                    unallowed[ThisKey(key)].Add("HoursCompleted");
                }
                
                unallowed = MergeDictionaries(unallowed, _userRules.GetUnallowed(condition, "CustomerContact"));
                unallowed = MergeDictionaries(unallowed, _userRules.GetUnallowed(condition, "ServiceLeader"));
                
                return unallowed;
            }
        }
    }
}