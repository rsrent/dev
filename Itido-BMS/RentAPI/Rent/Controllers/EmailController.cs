using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using Rent.EmailTemplates;
using Rent.Helpers;
using Rent.Models;
using Rent.Repositories;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers
{

    [Produces("application/json")]
    [Route("api/Email")]
    public class EmailController : Controller
    {
        private readonly RentContext _context;
        private readonly NotificationRepository _notificationRepository;
        private readonly SendGridEmail _sendGridEmail;
        private readonly NewsRepository _newsRepository;
        public EmailController(RentContext context, NotificationRepository notificationRepository, SendGridEmail sendGridEmail, NewsRepository newsRepository)
        {
            _context = context;
            _notificationRepository = notificationRepository;
            _sendGridEmail = sendGridEmail;
            _newsRepository = newsRepository;
        }

        [HttpPost("MailToOffice")]
        [Authorize]
        public async Task<IActionResult> SendMailToOffice([FromBody] EmailToOfficeDTO email)
        {
            var location = _context.Location.Find(email.LocationID);
            var user = _context.User.Find(_userID);

            if (location == null)
                return BadRequest("Location not found");
            if (user == null)
                return BadRequest("User not found");

            var header = "Besked fra medarbejder angående " + location.Name + (location.ProjectNumber != null ? " - " + location.ProjectNumber : "");
            var body = "Besked fra medarbejder: " + user.FirstName + " " + user.LastName + " " + user.EmployeeNumber;
            body += "\n Beskeden er angående " + location.Name + (location.ProjectNumber != null ? " med projektnummer: " + location.ProjectNumber : "");
            body += "\n Beskrivelse: \n" + email.Message;

            var log = new LocationLog { UserID = user.ID, LocationID = location.ID, DateCreated = DateTimeHelpers.GmtPlusOneDateTime(), Log = body, Title = header, CustomerCreated = true };
            _context.LocationLog.Add(log);
            await _context.SaveChangesAsync();
            await _sendGridEmail.Send("info@rs-rent.dk", header, body);
            return Ok();
        }

        [HttpGet("MailToOfficeFromWeb/{locationId}/{message}")]
        [Authorize]
        public async Task<IActionResult> SendMailToOfficeFromWeb([FromRoute] int locationId, [FromRoute] string message)
        {
            var location = _context.Location.Find(locationId);
            var user = _context.User.Find(_userID);

            if (location == null)
                return BadRequest("Location not found");
            if (user == null)
                return BadRequest("User not found");

            var header = "Besked fra kunde angående " + location.Name + (location.ProjectNumber != null ? " - " + location.ProjectNumber : "");
            var body = "Besked fra kunde: " + user.FirstName + " " + user.LastName;
            body += "\n Beskeden er angående " + location.Name + (location.ProjectNumber != null ? " med projektnummer: " + location.ProjectNumber : "");
            body += "\n Beskrivelse: \n" + message;
                
            var log = new LocationLog { UserID = user.ID, LocationID = location.ID, DateCreated = DateTimeHelpers.GmtPlusOneDateTime(), Log = message, Title = "Besked fra kunde", CustomerCreated = true };
            _context.LocationLog.Add(log);
            await _context.SaveChangesAsync();

            await _sendGridEmail.Send("info@rs-rent.dk", header, body);

            await _newsRepository.AddNews(_userID, locationId, "Ny besked til kontoret", message);

            return NoContent();
        }

        /*

        [HttpPost("EmailTobias")]
        public async Task<IActionResult> EmailTobias() 
        {
            var users = _context.User.Where(u => u.Email.Equals("todibbang@gmail.com")).ToList();
            SendEmailToUsers(users);
            return Ok(users);
        }

        [HttpPost("EmailSemllerUlla")]
        public async Task<IActionResult> EmailSemllerUlla()
        {
            var users = _context.User.Where(u => u.ID == 91).ToList();
            SendEmailToUsers(users);
            return Ok(users);
        }

        [HttpPost("EmailSEandScancoll")]
        public async Task<IActionResult> EmailSemllerKun() {

            var users = _context.User.Where(u => u.CustomerID == 75 || u.CustomerID == 87).ToList();
            SendEmailToUsers(users);
            return Ok(users);
        }

        [HttpPost("EmailHOGMLars")]
        public async Task<IActionResult> EmailHOGMLars()
        {
            var users = _context.User.Where(u => u.ID == 6).ToList();
            SendEmailToUsers(users);
            return Ok(users);
        }

        [HttpPost("EmailHOGMAnita837")]
        public async Task<IActionResult> EmailHOGMAnita837()
        {
            var users = _context.User.Where(u => u.ID == 13).ToList();
            SendEmailToUsers(users);
            return Ok(users);
        }

        [HttpPost("InviteHM")]
        public async Task<IActionResult> InviteHM()
        {
            var users = _context.User.Where(u => u.CustomerID == 1).ToList();
            await SendEmailToUsers(users, "K9FQ40X3");
            return Ok(users);
        }

        async Task SendEmailToUsers(ICollection<User> users, string standardPassword = "") 
        {
            string header = "Velkommen til Rengøringsselskabet Rent's nye samarbejdsplatform.";
            foreach (var user in users)
            {
                var login = _context.Login.Find(user.LoginID);

                var newPassword = RandomString(6);
                if(!string.IsNullOrEmpty(standardPassword))
                {
                    newPassword = standardPassword;
                }

                var newPasswordHashed = LoginRepository.HashPassword(newPassword);
                login.Password = newPasswordHashed;
                _context.Login.Update(login);
                _context.SaveChanges();
                await _notificationRepository.SendEmails(new List<User> { user }, header, CreateInviteEmail.Build(login.UserName, newPassword));
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        [HttpPost("InfromUsersOfSecurityChanges")]
        public async Task<IActionResult> InfromUsersOfSecurityChanges()
        {
            var users = _context.User.Where(u => u.CustomerID != 1 && u.CustomerID != 87 && u.CustomerID != null).ToList();
            foreach (var user in users)
            {
                var emailHeader = "Sikkerhedsopdatering";
                var emailBody = "Vi har opdateret vores sikkerhed. Det betyder at du skal logge ud og derefter ind igen, for at få adgang til systemet.";

                await _notificationRepository.SendEmails(new List<User> { user }, "Sikkerhedsopdatering - Rent", CreateNormalNotificationEmail.Build(emailHeader, emailBody));
            }
            return Ok(users);
        }

        //*/




        /*

        [HttpGet("MailToMudassar")]
        [AllowAnonymous]
        public async Task<IActionResult> MailToMudassar()
        {
            await QualityReportCompleted();
            await TaskCompleted1();
            await TaskCompleted2();
            return Ok();
        }

        public async Task QualityReportCompleted()
        {
            //var qualityReport = _context.QualityReport.Find(qualityReportID);
            //var location = _context.Location.Include(l => l.ServiceLeader).FirstOrDefault(l => l.ID == qualityReport.LocationID);
            //var location = _context.Location.FirstOrDefault(l => l.ID == qualityReport.LocationID);
            //var sl = LocationRepository.GetServiceLeadersStatic(location.ID, _context).First();

            var header = "Kvalitetsrapport klar for 700 Vejle Bryggen";
            var body =
                "Dato for næste besøg er 14-08-2018 .<br>" +
                "Kære kunde din mening er vigtig for os og for at vi kan blive endnu bedre. " +
                "Derfor sætter vi stor pris på at du vil give os din mening.";
            var htmlBody = CreateQualityReportCompletedTemplate.Build(header, body, "Rate/" + 0);

            var userToReceiveNotification = _context.User.Where(u => u.ID == 107 || u.ID == 428).ToList();
            //var userToReceiveNotification = LocationRepository.GetCustomerContactsStatic(location.ID, _context);
            await _sendGridEmail.Send(userToReceiveNotification, header, htmlBody.HtmlBody);
        }

        public async Task TaskCompleted1()
        {
            var header = "Opgave udført for 700 Vejle Bryggen";
            var body = "Opgaven Lukkede udstillingsvinduer indvendigt er blevet rengjort.<br>";
            body += "Opgaven blev udført af Husnain Asghar";
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            var userToReceiveNotification = _context.User.Where(u => u.ID == 107 || u.ID == 428).ToList();
            await _sendGridEmail.Send(userToReceiveNotification, header, htmlBody.HtmlBody);
        }

        public async Task TaskCompleted2()
        {
            var header = "Opgave udført for 700 Vejle Bryggen";
            var body = "Opgaven Fan coil er blevet rengjort.<br>";
            body += "Opgaven blev udført af Kiklos Vajo";
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            var userToReceiveNotification = _context.User.Where(u => u.ID == 107 || u.ID == 428).ToList();
            await _sendGridEmail.Send(userToReceiveNotification, header, htmlBody.HtmlBody);
        }

        */



        int _userID => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
