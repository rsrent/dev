using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Rent.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Rent.Helpers
{


    public class SendGridEmail
    {


        public async Task<SendGridEmailReport> Send(IEnumerable<User> users, string header, List<string> htmls)
        {
            SendGridEmailReport report = new SendGridEmailReport();


            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("rent-app@outlook.com", "Rent App"));

            var recipients = new List<EmailAddress>();
            foreach(var user in users) {
                if(IsValidEmail(user.Email))
                {
                    report.ValidEmails.Add(user.Email);
                    recipients.Add(new EmailAddress(user.Email, user.FirstName + " " + user.LastName));
                } else {
                    report.InvalidEmails.Add(user.Email);
                }
            }
            msg.AddTos(recipients);

            msg.SetSubject(header);

            htmls.ForEach((html) =>
            {
                msg.AddContent(MimeType.Html, html);
                report.body += "\n" + html;
            });


            var apiKey = "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";//Environment.GetEnvironmentVariable("RentEmailApiKey");
            var client = new SendGridClient(apiKey);


            var response = await client.SendEmailAsync(msg);






            report.StatusCode = response.StatusCode;
            return report;
        }

        public async Task<(bool, string)> Send(string recieverEmail, string header, string html)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("rent-app@outlook.com", "Rent App"));
            msg.AddTo(new EmailAddress(recieverEmail, "Fakturering"));

            msg.SetSubject(header);

            msg.AddContent(MimeType.Html, html);

            var apiKey = "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";//Environment.GetEnvironmentVariable("RentEmailApiKey");
            var client = new SendGridClient(apiKey);

            var response = await client.SendEmailAsync(msg);

            return (true, response.ToString());
        }

        bool IsValidEmail(string email) {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
