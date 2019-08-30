using Microsoft.Graph;
using Rent.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Rent.Outlook
{
    public class CreateMeeting
    {
        public static async Task<string> ScheduleMeeting(OutlookEventDTO eventDTO)
        {
            GraphServiceClient client = new GraphServiceClient(
            new DelegateAuthenticationProvider(
                (requestMessage) =>
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", eventDTO.Token);

                    requestMessage.Headers.Add("X-AnchorMailbox", eventDTO.Email);

                    return Task.FromResult(0);
                }));
            var myEvent = new Event
            {
                Subject = eventDTO.Subject,
                Start = new DateTimeTimeZone {
                    DateTime = eventDTO.Start.ToString(),
                    TimeZone = eventDTO.TimeZone ?? "Europe/Berlin"
                },
                End = new DateTimeTimeZone
                {
                    DateTime = eventDTO.End.ToString(),
                    TimeZone = eventDTO.TimeZone ?? "Europe/Berlin"
                },
            };
            var request = await client.Me.Events.Request().AddAsync(myEvent);
            return request.ICalUId;
        }
    }
}
