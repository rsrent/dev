using Microsoft.Graph;
using Rent.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Rent.Outlook
{
    public static class SendEmail
    {
        public static async Task SendMessage(OutlookEmailSendDTO emailDTO)
        {
            GraphServiceClient client = new GraphServiceClient(
            new DelegateAuthenticationProvider(
                (requestMessage) =>
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", emailDTO.Token);

                    requestMessage.Headers.Add("X-AnchorMailbox", emailDTO.Email);

                    return Task.FromResult(0);
                }));
            var message = new Message
            {
                Body = new ItemBody
                {
                    Content = emailDTO.Content,
                    ContentType = emailDTO.ContentType
                },
                Subject = emailDTO.Subject,
                ToRecipients = emailDTO.ToRecipients
            };
            await client.Me.SendMail(message).Request().PostAsync();
        }
    }
}
