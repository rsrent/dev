using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit;
using Rent.Data;
using Rent.EmailTemplates;
using Rent.Helpers;
using Rent.Models;
using Rent.PushNotifications;
using Rent.Repositories;
using Rent.Scheduler;
using Rent.Scheduler.Cron;


namespace Rent
{
    public class CleaningTaskReport : IScheduledTask
    {
        private readonly IServiceScopeFactory scopeFactory;
        private NotificationRepository _notificationRepository;

        public CleaningTaskReport(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public string Schedule => "0 12 * * 1-5";

        // ffj@rs-rent.dk

        // kritiske og overskredne vagter

        public async Task ExecuteAsync(CancellationToken cancellationToken) 
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<RentContext>();
                _notificationRepository = scope.ServiceProvider.GetService<NotificationRepository>();

                var frederik = _context.User.Where(u => u.ID == 473).ToList();

                BodyBuilder bodyBuilder = new BodyBuilder();
                bodyBuilder.HtmlBody = GetOverdueWindowTasks(_context);
                await _notificationRepository.SendEmails(frederik, "Rent opgave oversigt ", bodyBuilder);

                _context = null;
                _notificationRepository = null;
            }
        }

        string GetOverdueWindowTasks(RentContext context)
        {
            var twoDaysAgo = DateTime.Now.AddDays(-2).Ticks;

            var text = "";

            var tasks = context
                .CleaningTask
                .Where(t => t.Active && !t.Location.Disabled && !t.Location.Customer.Disabled && t.Area.CleaningPlanID == 2)
                .Include(c => c.LastTaskCompleted)
                .Include(c => c.Location)
                .ThenInclude(l => l.Customer)
                .Include(c => c.Location)
                .ThenInclude(l => l.LocationUsers)
                .ThenInclude(lu => lu.User)
                .Include(c => c.Location)
                .ThenInclude(l => l.LatestQualityReport)
                .Include(ct => ct.CompletedTasks)
                .ThenInclude(cct => cct.CompletedByUser)
                .Include(ct => ct.Area)
                .ThenInclude(a => a.CleaningPlan)
                .Include(ct => ct.Floor)
                .Where(ct => ct.TimesOfYear != null)
                .Where(ct => ct.LastTaskCompleted != null && ct.LastTaskCompleted.CompletedDate.Ticks + ((365.0 / ct.TimesOfYear) * 864000000000) < twoDaysAgo)
                .ToList();

            tasks.ForEach((t) =>
            {
                text += t.Location.Name + ", " + t.Location.Customer.Name + ", " +t.Area.Description + ": " + (t.LastTaskCompleted.CompletedDate.AddDays((365.0 / (double)t.TimesOfYear))).ToString("dd-MM-yy") + "<br><br>";
            });

            return text;
        }
    }
}
