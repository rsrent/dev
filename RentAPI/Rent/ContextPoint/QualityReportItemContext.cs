using System;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;

namespace Rent.ContextPoint
{
    public class QualityReportItemContext : Context<QualityReportItem>
    {
        public QualityReportItemContext(RentContext rentContext) : base(rentContext) { }

        protected override DbSet<QualityReportItem> GetDb() => RentContext.QualityReportItem;

        protected override string Permission() => "QualityReport";
    }
}
