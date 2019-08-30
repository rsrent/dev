using Rent.DTOs;
using Rent.Helpers;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeKit;
using Rent.EmailTemplates;
using Rent.Data;
using Microsoft.EntityFrameworkCore;
using Rent.PushNotifications;
using Rent.ContextPoint;


namespace Rent.Repositories
{
    public class NotificationRepository
    {
        //ICollection<INotificationChannel> notificationChannels = new List<INotificationChannel>();

        EmailNotificationChannel _emailChannel;
        PushNotificationController _phoneChannel;
        SendGridEmail _sendGridEmail;
        RentContext _context;
        LocationContext _locationContext;
        NewsRepository _newsRepository;

        public NotificationRepository(RentContext context, LocationContext locationContext, EmailNotificationChannel emailChannel, PushNotificationController phoneChannel, SendGridEmail sendGridEmail, NewsRepository newsRepository)
        {
            _context = context;
            _locationContext = locationContext;

            _emailChannel = emailChannel;
            _phoneChannel = phoneChannel;
            _sendGridEmail = sendGridEmail;
            _newsRepository = newsRepository;
            //_locationRepository = locationRepository;
        }

        public async void QualityReportCompleted(int qualityReportID, DateTime nextReportDate, int ratingValue)
        {
            var qualityReport = _context.QualityReport.Find(qualityReportID);
            //var location = _context.Location.Include(l => l.ServiceLeader).FirstOrDefault(l => l.ID == qualityReport.LocationID);
            var location = _context.Location.FirstOrDefault(l => l.ID == qualityReport.LocationID);
            //var sl = LocationRepository.GetServiceLeadersStatic(location.ID, _context).First();

            var header = "Kvalitetsrapport klar for " + location.Name;
            var body =
                "Dato for næste besøg er " + nextReportDate.ToString("dd-MM-yyyy") + ".<br>" +
                "Kære kunde din mening er vigtig for os og for at vi kan blive endnu bedre. " +
                "Derfor sætter vi stor pris på at du vil give os din mening.";
            var htmlBody = CreateQualityReportCompletedTemplate.Build(header, body, "Rate/" + qualityReport.RatingID);

            //var userToReceiveNotification = _context.User.Find(location.CustomerContactID);

            var cc = _locationContext.Database(0, l => l.ID == location.ID, "LocationUsers").Select(l => l.CustomerContact).ToList();

            //var userToReceiveNotification = LocationRepository.GetCustomerContactsStatic(location.ID, _context);
            var response = await SendNotifications(cc, header, body, htmlBody);

            var feedbackHeader = header + " - " + response.StatusCode;
            var feedbackBody = "";

            feedbackBody += "Status code: " + response.StatusCode + "\n";
            feedbackBody += "Valide emails: ";
            response.ValidEmails.ForEach((validEmail) =>
            {
                feedbackBody += validEmail;
                feedbackBody += ", ";
            });
            feedbackBody += "Invalide emails: ";
            response.InvalidEmails.ForEach((invalidEmail) =>
            {
                feedbackBody += invalidEmail;
                feedbackBody += ", ";
            });

            feedbackBody += " -- Email sendt:\n";
            feedbackBody += response.body;

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted || response.InvalidEmails.Count > 0)
            {
                var emails = "\nValide emails: ";
                response.ValidEmails.ForEach((validEmail) =>
                {
                    emails += validEmail;
                    emails += ", ";
                });
                emails += "\nIkke valide emails: ";
                response.InvalidEmails.ForEach((invalidEmail) =>
                {
                    emails += invalidEmail;
                    emails += ", ";
                });
                await _newsRepository.AddNews(0,
                                              location.ID,
                                              response.StatusCode + " - Fejl ved fremsendelse af email om udført kvalitetsrapport - " + location.Name,
                                              "Der skete en fejl ved fremsendelse af email om udført kvalitetsrapport til følgende emails: \n" + emails);
            }

            await _sendGridEmail.Send("rent-app@outlook.com", feedbackHeader, feedbackBody);
        }

        public async void TaskCompleted(int taskCompletedID)
        {
            var taskCompleted = _context.CleaningTaskCompleted.Find(taskCompletedID);
            var task = _context.CleaningTask.Include(t => t.Area).FirstOrDefault(t => t.ID == taskCompleted.CleaningTaskID);
            var location = _context.Location.Find(task.LocationID);
            var user = _context.User.Find(taskCompleted.CompletedByUserID);

            var header = "Opgave udført for " + location.Name;
            var body = "Området " + task.Area.Description + " " + task.Comment + " er blevet rengjort.<br>";
            body += "Opgaven blev udført af " + user.FirstName + " " + user.LastName;
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            //var userToReceiveNotification = _context.User.Find(location.CustomerContactID);
            //var userToReceiveNotification = LocationRepository.GetCustomerContactsStatic(location.ID, _context);

            var cc = _locationContext.Database(0, l => l.ID == location.ID, "LocationUsers").Select(l => l.CustomerContact).ToList();

            await SendNotifications(cc, header, body, htmlBody);
        }

        public async void MoreWorkOrdered(int moreworkID)
        {
            var morework = _context.MoreWork.Find(moreworkID);
            var location = _context.Location.Find(morework.LocationID);
            var user = _context.User.Find(morework.CreatedByUserID);
            var header = "Merarbejde bestilling for projekt" + (location.ProjectNumber != null ? " " + location.ProjectNumber : "") + " gennem appen.";
            var body = user.FirstName + " " + user.LastName + " har bestilt merarbejde til lokationen " + location.Name + (location.ProjectNumber != null ? " med projektnummer: " + location.ProjectNumber : "");
            body += "Merarbejdedetaljerne er som føljer: ";
            body += "\n Beskrivelse: " + morework.Description;
            body += "\n Udførelsestid: " + morework.ExpectedCompletedTime.ToString();
            await _sendGridEmail.Send("info@rs-rent.dk", header, body);
        }

        public async void MoreWorkCompletedd(int moreworkID)
        {
            var morework = _context.MoreWork.Find(moreworkID);
            var location = _context.Location.Find(morework.LocationID);
            var user = _context.User.Find(morework.CreatedByUserID);
            var header = "Merarbejde udført for projekt" + (location.ProjectNumber != null ? " " + location.ProjectNumber : "") + " gennem appen.";
            var body = user.FirstName + " " + user.LastName + " har bestilt merarbejde til lokationen " + location.Name + (location.ProjectNumber != null ? " med projektnummer: " + location.ProjectNumber : "");
            body += "Merarbejdedetaljerne er som føljer: ";
            body += "\n Beskrivelse: " + morework.Description;
            body += "\n Udførelsestid: " + morework.ActualCompletedTime.ToString();
            body += "\n Tid brugt: " + morework.Hours;
            await _sendGridEmail.Send("salgsfakturering@rs-rent.dk", header, body);
        }


        public void StaffAdded(int locationID, string staffName)
        {
            return;
            var location = _context.Location.Find(locationID);
            var header = "Personalestab for " + location.Name + " er opdateret";
            var body = staffName + " er blevet tilføjet til personalestaben for " + location.Name + ". For mere information, gå til web-appen hvor du altid kan se personalet mm.";
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            var users = _context.LocationUser.Include(lu => lu.User).Where(lu => lu.User.CustomerID == location.CustomerID).Select(lu => lu.User);
            SendNotifications(users.ToList(), header, body, htmlBody);
        }

        public void StaffRemoved(int locationID, string staffName)
        {
            return;
            var location = _context.Location.Find(locationID);
            var header = "Personalestab for " + location.Name + " er opdateret";
            var body = staffName + " er ikke længere en del af personalestaben for " + location.Name + ". For mere information, gå til web-appen hvor du altid kan se personalet mm.";
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            var users = _context.LocationUser.Include(lu => lu.User).Where(lu => lu.User.CustomerID == location.CustomerID).Select(lu => lu.User);
            SendNotifications(users.ToList(), header, body, htmlBody);
        }

        public void ServiceLeaderUpdated(int locationID, string staffName)
        {
            return;
            var location = _context.Location.Find(locationID);
            var header = location.Name + " har fået en ny serviceleder";
            var body = staffName + " er den nye serviceleder for " + location.Name + ". For mere information, gå til web-appen hvor du altid kan se personalet mm.";
            var htmlBody = CreateNormalNotificationEmail.Build(header, body);

            var users = _context.LocationUser.Include(lu => lu.User).Where(lu => lu.User.CustomerID == location.CustomerID).Select(lu => lu.User);
            SendNotifications(users.ToList(), header, body, htmlBody);
        }

        public async Task<SendGridEmailReport> SendNotifications(IEnumerable<User> users, string title, string body, BodyBuilder htmlBody)
        {
            var response = (await _sendGridEmail.Send(users, title, new List<string>() { htmlBody.HtmlBody }));

            foreach (var u in users)
            {
                await Task.Run(() => _phoneChannel.Send(u.ID, title, body));
                //await Task.Run(() => _emailChannel.Send(u.Email, title, htmlBody));
            }

            return response;
        }

        public async Task SendEmails(IEnumerable<User> users, string title, BodyBuilder htmlBody)
        {
            await _sendGridEmail.Send(users, title, new List<string>() { htmlBody.HtmlBody });
        }
    }
}
