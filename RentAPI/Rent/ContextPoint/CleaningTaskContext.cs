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
    public class CleaningTaskContext : ContextDto<CleaningTask>
    {
        public CleaningTaskContext(RentContext rentContext, PropCondition condition) : base(rentContext, condition) { }

        protected override string Permission() => "CleaningTask";
        
        protected override DbSet<CleaningTask> GetDb() => RentContext.CleaningTask;
        
        protected override IQueryable<CleaningTask> SpecialGetRequirement(int requester, IQueryable<CleaningTask> query)
        {
            var plansToFetch = new List<int>();
            var stringPermissions = GetUserPermissions(requester);

            foreach (var up in stringPermissions)
            {
                
                if (up.Permission.Name == "RegularTask" && up.Read)
                    plansToFetch.AddRange(new[] { 1, 4 });
                if (up.Permission.Name == "WindowTask" && up.Read)
                    plansToFetch.Add(2);
                if (up.Permission.Name == "FanCoilTask" && up.Read)
                    plansToFetch.Add(3);
            }

            return query.Where(ct => ct.Active && plansToFetch.Contains(ct.Area.CleaningPlanID));
        }
        
        public override ContextRules GetRules() => new Rules();
        
        public class Rules : ContextRules
        {
            public override string ThisKey(string key = null) => key?? "CleaningTask";

            public Rules() { }
        
            public override Dictionary<string, List<string>> GetUnallowed(PropCondition condition, string key = null)
            {
                var unallowed = new Dictionary<string, List<string>> {{ThisKey(key), new List<string>()}};

                return unallowed;
            }
        }
    }
}
