using System;
using MailKit;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace ModuleLibraryiOS.Services
{
    public class Mail
    {

        string User;
        string Password;
        string SenderName;

        public Mail(string user, string password, string senderName) {
            User = user;
            Password = password;
            SenderName = senderName;
        }
        
        public async Task<bool> PostMail(string mailTo, string receiverName, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(SenderName, User));
            message.To.Add(new MailboxAddress(receiverName, mailTo));
            message.Subject = subject;

            message.Body = new TextPart("plain")
            {
                Text = @"" + body
            };
            try{
				using (var client = new SmtpClient())
				{
					// For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
					client.ServerCertificateValidationCallback = (s, c, h, e) => true;

					client.Connect("smtp.gmail.com", 465, true);

					// Note: since we don't have an OAuth2 token, disable
					// the XOAUTH2 authentication mechanism.
					client.AuthenticationMechanisms.Remove("XOAUTH2");

					// Note: only needed if the SMTP server requires authentication
					//client.Authenticate("SickMailService@gmail.com","ServiceTest");
					client.Authenticate(User, Password);

					await client.SendAsync(message);
					client.Disconnect(true);
				}
            } catch {
                return false;
            }
            return true;
        }
    }
}
