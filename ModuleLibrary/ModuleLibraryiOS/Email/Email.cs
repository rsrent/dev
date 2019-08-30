using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using ModuleLibrary.Shared.Email;
using ModuleLibraryShared.Email;

namespace ModuleLibraryiOS.Email
{
    public class Email : IEmail
    {
        //private readonly IEmailSettings _settings;


        public Email(IEmailSettings settings) : base(settings)
        {
            //_settings = settings;
        }
 /*
        public async Task<bool> Send(int receiverID, string header, string body)
        {
            var details = _settings.ReceiverDetails(receiverID);

            if (details == null)
                return false;

            string name = details[1];
            string email = details[0];

            return await Send(email, name, header, body);
        }
*/
        public override async Task<bool> Send(string receiverMail, string receiverName, string header, string body, ICollection<EmailAttachment> attachments = null) {

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.SenderName(), _settings.SenderEmail()));
            message.To.Add(new MailboxAddress(receiverName, receiverMail));
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
                return false;
            }
            return true;
        }
    }
}
