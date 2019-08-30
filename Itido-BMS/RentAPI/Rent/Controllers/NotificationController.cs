using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rent.DTOs;
using Rent.Helpers;
using Rent.PushNotifications;
using Rent.Repositories;

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Notification")]
    public class NotificationController : Controller
    {
        PushNotificationController _phoneChannel;

        public NotificationController(PushNotificationController phoneChannel) 
        {
            _phoneChannel = phoneChannel;
        }

        [HttpPost("register/{userID}/{devicePlatform}")]
        [Authorize]
        public async Task<IActionResult> Register(int userID, string devicePlatform, [FromBody] PushNotifications.NotificationTokenDTO notificationTokenDTO)
        {
            var success = await _phoneChannel.Register(userID, devicePlatform, notificationTokenDTO);
            if(success)
            {
                return Ok();
            }
            return BadRequest("Error connecting to notifcationServer");
        }
    }
}