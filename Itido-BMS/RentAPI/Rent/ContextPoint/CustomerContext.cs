using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Graph;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class CustomerContext : ContextDto<Customer>
    {
        private readonly UserContext.Rules _userRules;
        public CustomerContext(RentContext rentContext, PropCondition condition, UserContext.Rules userRules) : base(rentContext, condition)
        {
            _userRules = userRules;
        }

        protected override string Permission() => "Customer";

        protected override DbSet<Customer> GetDb() => RentContext.Customer;

        protected override IQueryable<Customer> SpecialGetRequirement(int requester, IQueryable<Customer> query)
        {
            var canViewDisabled = !Unauthorized(requester, Permission(), CRUDD.Delete);
            query = query.Where(u => (!u.Disabled || canViewDisabled));

            if (requester != 0)
            {
                var user = RentContext.User.Find(requester);
                if (user.CustomerID != null)
                    query = query.Where(c => c.ID == user.CustomerID);
            }

            return query;
        }

        public override ContextRules GetRules()
        {
            return new Rules(_userRules);
        }


        public Dictionary<string, List<string>> GetUnallowed(PropCondition condition, string key = "Customer")
        {
            var unallowed = new Dictionary<string, List<string>>();

            unallowed.Add(key, new List<string>());

            if (condition.IsEmployee())
            {
                unallowed[key].Add("Comment");
            }



            return unallowed;
        }
        
        public class Rules : ContextRules
        {
            public override string ThisKey(string key = null) => key?? "Customer";
            
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

                unallowed = MergeDictionaries(unallowed, _userRules.GetUnallowed(condition, "MainUser"));
                unallowed = MergeDictionaries(unallowed, _userRules.GetUnallowed(condition, "KeyAccountManager"));
                
                /*
                foreach (var k in unallowed.Keys)
                {
                    Console.WriteLine(k);
                    foreach (var v in unallowed[k])
                    {
                        Console.WriteLine("   " + v);
                    }
                }
                */
                return unallowed;
            }
        }
    }
}
