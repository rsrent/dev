using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Rent.DTOs;

namespace Rent.Helpers
{
    /*
    public class PhoneNotificationChannel : INotificationChannel
    {
        IPhoneNotificationSettings _settings;
        public PhoneNotificationChannel(IPhoneNotificationSettings settings)
        {
            _settings = settings;
        }

        public async Task<(bool, string)> Send(int receiverID, string title, string body)
        {
            return (await new HttpCall.CallManager().Call(HttpCall.CallType.Post, _settings.GetConnectionString() + "Send?tag=_profile_" + receiverID + "&title=" + title + "&body=" + body), "ok");
        }

        public async Task<bool> Register(int userID, string devicePlatform, NotificationTokenDTO notificationTokenDTO)
        {
            var success = await new HttpCall.CallManager().Call(HttpCall.CallType.Post, _settings.GetConnectionString() + "Register?tag=_profile_" + userID + "&devicePlatform=" + devicePlatform, notificationTokenDTO);

            if (success)
            {
                return true;
            }
            return false;
        }
    }
    */
}
