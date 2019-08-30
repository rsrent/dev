using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Rent.Data;


namespace Rent.Helpers
{
    public class EmailNotificationChannel : INotificationChannel
    {
        private readonly IEmailNotificationChannelSettings _settings;

        public EmailNotificationChannel(IEmailNotificationChannelSettings settings)
        {
            _settings = settings;
        }

        public async Task<(bool, string)> Send(int receiverID, string header, string body)
        {
            var details = _settings.ReceiverDetails(receiverID);

            if (details == null)
                return (false, "Receiver not found");

            string name = details[1];
            string email = details[0];

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName(), _settings.SenderEmail()));
            message.To.Add(new MailboxAddress(name, email));
            message.Subject = header;

            message.Body = new TextPart("plain") { Text = @"" + body };

            try
            {
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;

                    client.Connect(_settings.Protocol(), _settings.Port(), SecureSocketOptions.Auto);

                    // Note: since we don't have an OAuth2 token, disable
                    // the XOAUTH2 authentication mechanism.

                    //client.AuthenticationMechanisms.Remove("XOAUTH2");

                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(_settings.SenderEmail(), _settings.SenderPassword());

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            return (true, "ok");
        }

        public async Task<(bool, string)> Send(string receiverEmail, string header, BodyBuilder body)
        {
            //var addr = "";
            try
            {
                var addr = new System.Net.Mail.MailAddress(receiverEmail);
                if(addr.Address != receiverEmail) {
                    return (false, "not correct email");
                }
            }
            catch
            {
                return (false, "not correct email");
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName(), _settings.SenderEmail()));
            message.To.Add(new MailboxAddress("Receiver", receiverEmail));
            message.Subject = header;

            message.Body = body.ToMessageBody();

            try
            {
                using (var client = new SmtpClient())
                {
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(_settings.Protocol(), _settings.Port(), SecureSocketOptions.Auto);
                    client.Authenticate(_settings.SenderEmail(), _settings.SenderPassword());

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }
            return (true, "ok");
        }
    }
}
