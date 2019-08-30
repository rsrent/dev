using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using Newtonsoft.Json;
using Rent.Helpers;

namespace Rent.PushNotifications
{
    public class PushNotificationController : INotificationChannel
    {
        private NotificationHubClient _hub;

        public PushNotificationController(IPhoneNotificationSettings settings)
        {
            _hub = NotificationHubClient.CreateClientFromConnectionString(settings.GetConnectionString(), settings.GetNotificationHubPath());

            //_hub = NotificationHubClient.CreateClientFromConnectionString("Endpoint=sb://rentapp.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=2mMBQM+TdDWvCwM/EAy8yxQPwTjMYzepO8rcPJCa46s=",
            //                                                             "rentNotifications");
        }

        public async Task<(bool, string)> Send(int recieverID, string title, string body)
        {
            var notification = new
            {
                aps = new
                {
                    alert = new
                    {
                        title,
                        body
                    }
                }
            };

            var alert = JsonConvert.SerializeObject(notification);
            try{
                var result = await _hub.SendAppleNativeNotificationAsync(alert, "_profile_" + recieverID);
            } catch (Exception e) {
                Console.WriteLine(e);
            }


            return (true, "Ok");
        }

        public async Task<object> sendWithContent(string tag, string title, string body, string type, int id)
        {
            var notification = new
            {
                aps = new
                {
                    alert = new
                    {
                        title,
                        body
                    },
                    content = new
                    {
                        type,
                        id
                    }
                }
            };

            var alert = JsonConvert.SerializeObject(notification);
            try
            {
                var result = await _hub.SendAppleNativeNotificationAsync(alert, tag);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return (true, "Ok");
        }

        public async Task<bool> Register(int id, string devicePlatform, NotificationTokenDTO notificationToken)
        {
            var registrationId = await GetRegistrationId(notificationToken.DeviceToken);

            RegistrationDescription registration = null;

            switch (devicePlatform)
            {
                case "mpns":
                    registration = new MpnsRegistrationDescription(notificationToken.DeviceToken);
                    break;
                case "wns":
                    registration = new WindowsRegistrationDescription(notificationToken.DeviceToken);
                    break;
                case "apns":
                    registration = new AppleRegistrationDescription(notificationToken.DeviceToken);
                    break;
                case "gcm":
                    registration = new GcmRegistrationDescription(notificationToken.DeviceToken);
                    break;
                default:
                    return false;
            }

            registration.RegistrationId = registrationId;
            registration.Tags = new HashSet<string>();

            registration.Tags.Add("_profile_" + id);

            try
            {
                await _hub.CreateOrUpdateRegistrationAsync(registration);
            }
            catch (MessagingException e)
            {
                ReturnGoneIfHubResponseIsGone(e);
            }

            return true;
        }

        public async Task<string> GetRegistrationId(string deviceToken)
        {
            string newRegistrationId = null;

            // make sure there are no existing registrations for this push handle (used for iOS and Android)
            if (deviceToken != null)
            {
                var registrations = await _hub.GetRegistrationsByChannelAsync(deviceToken, 100);

                foreach (RegistrationDescription registration in registrations)
                {
                    if (newRegistrationId == null)
                    {
                        newRegistrationId = registration.RegistrationId;
                    }
                    else
                    {
                        await _hub.DeleteRegistrationAsync(registration);
                    }
                }
            }

            if (newRegistrationId == null)
            {
                newRegistrationId = await _hub.CreateRegistrationIdAsync();
            }

            return newRegistrationId;
        }

        private static void ReturnGoneIfHubResponseIsGone(MessagingException e)
        {
            var webex = e.InnerException as WebException;
            if (webex.Status == WebExceptionStatus.ProtocolError)
            {
                var response = (HttpWebResponse)webex.Response;
                if (response.StatusCode == HttpStatusCode.Gone)
                    throw new HttpRequestException(HttpStatusCode.Gone.ToString());
            }
        }
    }
}
