using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
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
	public class UpdatePasswordsSchedule : IScheduledTask
    {
        private readonly IServiceScopeFactory scopeFactory;

        private NotificationRepository _notificationRepository;

        public UpdatePasswordsSchedule(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        public string Schedule => "0 12 * * 1";

        public async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var _context = scope.ServiceProvider.GetRequiredService<RentContext>();
                _notificationRepository = scope.ServiceProvider.GetService<NotificationRepository>();
                var potentialCustomers =
                    _context.Customer.Where(c => c.UpdatePasswordsAtSecondsInterval != null).ToList();

                foreach (var c in potentialCustomers)
                {
                    if (c.PasswordsLastUpdated.AddSeconds((int) c.UpdatePasswordsAtSecondsInterval) < DateTime.UtcNow)
                    {
                        await UpdateCusomerPasswords(_context, c);
                    }
                }
                _context = null;
                _notificationRepository = null;
            }
        }

        async Task test(RentContext _context, Customer c)
        {
            System.Diagnostics.Debug.WriteLine(c.Name + " was updated");

            c.PasswordsLastUpdated = DateTime.UtcNow;
            _context.Customer.Update(c);
            _context.SaveChanges();
        }

        async Task UpdateCusomerPasswords(RentContext _context, Customer c) 
        {
            var customerUsers = _context.User.Where(u => u.CustomerID == c.ID && !u.Disabled || u.ID == 177).ToList();
            await SendEmailToUsers(_context, customerUsers);
            c.PasswordsLastUpdated = DateTime.UtcNow;
            _context.Customer.Update(c);
            await _context.SaveChangesAsync();
        }

        async Task SendEmailToUsers(RentContext _context, ICollection<User> users)
        {
            string header = "Dit Rent kodeord er blevet opdateret.";
            foreach (var user in users)
            {
                var login = _context.Login.Find(user.LoginID);
                var newPassword = RandomString(8);
                var newPasswordHashed = LoginRepository.HashPassword(newPassword);
                login.Password = newPasswordHashed;
                _context.Login.Update(login);
                _context.SaveChanges();

                var emailHeader = "Dit kodeord til din rent konto er blevet opdateret";
                var emailBody = "Dit nye kodeord er " + newPassword + ".";

                await _notificationRepository.SendEmails(new List<User> { user }, header, CreateNormalNotificationEmail.Build(emailHeader, emailBody));
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
