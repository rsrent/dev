using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using Rent.Data;
using Rent.EmailTemplates;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Announcement")]

    public class AnnouncementController : Controller
    {
        private readonly RentContext _context;
        private readonly NotificationRepository _notificationRepository;


        public AnnouncementController(RentContext context, NotificationRepository notificationRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
        }

        [HttpPut("EmailToCustomerUsers/{customerId}/{header}/")]
        public async Task<IActionResult> SendEmailToCustomer([FromRoute] int customerId, [FromRoute] string header, [FromBody] string body) 
        {
            var cutomer = _context.Customer.Find(customerId);
            if (cutomer == null)
                return NotFound("Customer not found");

            var users = _context.User.Where(u => u.CustomerID == cutomer.ID);

            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            await _notificationRepository.SendNotifications(users.ToList(), header, body, htmlBody);

            return Ok();
        }
    }
}
