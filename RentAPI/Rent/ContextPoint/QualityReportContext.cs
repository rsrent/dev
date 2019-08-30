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
    public class QualityReportContext : ContextDto<QualityReport>
    {
        public QualityReportContext(RentContext rentContext, PropCondition condition) : base(rentContext, condition) { }

        protected override string Permission() => "QualityReport";

        protected override DbSet<QualityReport> GetDb() => RentContext.QualityReport;


        public override ContextRules GetRules() => new Rules();

        public class Rules : ContextRules
        {
            public override string ThisKey(string key = null) => key?? "QualityReport";
            
            public Rules() { }
        
            public override Dictionary<string, List<string>> GetUnallowed(PropCondition condition, string key = null)
            {
                var unallowed = new Dictionary<string, List<string>> {{ThisKey(key), new List<string>()}};

                return unallowed;
            }
        }
    }
}
