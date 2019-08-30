//TODO REMOVE
/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Azure.NotificationHubs.Messaging;
using System.Web;
using Rent.Data;
using Rent.DTOs;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Register")]
    public class RegisterController : Controller
    {
        private readonly RentContext _context;

        private NotificationHubClient _hub;

        public RegisterController(RentContext context)
        {
            _hub = Notifications.Notifications.Instance.Hub;
            _context = context;
        }
        
        // POST: api/Register/apns/User/1
        [HttpPost("{devicePlatform}/User/{userID}")]
        public async Task<IActionResult> RegisterForNotification(
            [FromRoute] string devicePlatform, 
            [FromRoute] int userID, 
            [FromBody] NotificationTokenDTO notificationToken)
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
                    return BadRequest();
            }

            registration.RegistrationId = registrationId;
            registration.Tags = new HashSet<string>();


            registration.Tags.Add("_profile_" + userID);

            return Ok("You will now recieve notifications");
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
    }
}
*/