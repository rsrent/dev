using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Helpers;
using Rent.Models.Important;

namespace Rent.Repositories
{
    public class NewsRepository
    {
        private readonly RentContext _context;

        public NewsRepository(RentContext context)
        {
            _context = context;
        }

        public async Task AddNews(int requester, int locationId, string title, string body)
        {
            var user = _context.User.Find(requester);
            var location = await _context.Location.Include(l => l.Customer).FirstOrDefaultAsync(l => l.ID == locationId);

            var whereAndBy = $"{location.Name}, {location.Customer.Name} af {user.FirstName} {user.LastName}";

            var news = new News()
            {
                UserId = requester,
                LocationId = locationId,
                SubjectId = 0,
                Category = NewsCategory.Other,
                Time = DateTimeHelpers.GmtPlusOneDateTime(),
                Title = title,
                Body = body + "\n" + whereAndBy,
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
        }

        public async Task AddNews(int requester, int locationId, NewsCategory category, int subjectId)
        {
            var title = "";
            var body = "";

            var user = _context.User.Find(requester);
            var location = await _context.Location.Include(l => l.Customer).FirstOrDefaultAsync(l => l.ID == locationId);

            var whereAndBy = $"\n{location.Name}, {location.Customer.Name} af {user.FirstName} {user.LastName}";

            switch (category) {
                case NewsCategory.MoreWorkOrdered :
                    title = $"Merarbejde bestilt til ";
                    body = MoreWorkOrderedBody(subjectId) + whereAndBy;
                    break;
                case NewsCategory.MoreWorkCompleted:
                    title = $"Merarbejde udført til ";
                    body = MoreWorkCompletedBody(subjectId) + whereAndBy;
                    break;
                case NewsCategory.QualityReportStarted:
                    title = $"Kvalitetsrapport oprettet til ";
                    body = QualityReportStartedBody(subjectId) + whereAndBy;
                    break;
                case NewsCategory.QualityReportCompleted:
                    title = $"Kvalitetsrapport færdiggjort til ";
                    body = QualityReportCompletedBody(subjectId) + whereAndBy;
                    break;
                case NewsCategory.FanCoilTaskCompleted:
                    title = $"Fan coil opgave udført til ";
                    body = await CleaningTaskCompletedBody(subjectId) + whereAndBy;
                    break;
                case NewsCategory.WindowTaskCompleted:
                    title = $"Vindues opgave udført til ";
                    body = await CleaningTaskCompletedBody(subjectId) + whereAndBy;
                    break;
            }

            var news = new News()
            {
                UserId = requester,
                LocationId = locationId,
                SubjectId = subjectId,
                Category = category,
                Time = DateTimeHelpers.GmtPlusOneDateTime(),
                Title = title,
                Body = body,
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();
        }

        string cleaningPlanString(int id) => id == 1 ? "Rengørings" : id == 2 ? "Vindues" : id == 3 ? "Fan coil" : "Periodisk rengørings";

        string MoreWorkOrderedBody(int id)
        {
            var morework = _context.MoreWork.Find(id);

            var plantype = cleaningPlanString(morework.CleaningPlanID);

            return $"{plantype} opgave er blevet bestilt. Ønsked udført den {morework.ExpectedCompletedTime.ToString("dd-MM-yy")}. Bestilling er sket med kommentaren: {morework.Description}";
        }

        string MoreWorkCompletedBody(int id)
        {
            var morework = _context.MoreWork.Find(id);

            var plantype = morework.CleaningPlanID == 1 ? "Rengørings" : morework.CleaningPlanID == 2 ? "Vindues" : morework.CleaningPlanID == 3 ? "Fan coil" : "Periodisk rengørings";

            return $"{plantype} opgave er blevet udført med et timeforbrug på {morework.Hours}. Bestillingen kom med kommentaren: {morework.Description}";
        }

        string QualityReportStartedBody(int id)
        {
            //var report = _context.QualityReport.Find(id);
            return $"Kvalitetsrapport oprettet.";
        }

        string QualityReportCompletedBody(int id)
        {
            //var report = _context.QualityReport.Find(id);
            return $"Kvalitetsrapport færdiggjort";
        }

        async Task<string> CleaningTaskCompletedBody(int id)
        {
            var taskCompleted = await _context.CleaningTaskCompleted.Include("CleaningTask.Area.CleaningPlan").FirstOrDefaultAsync(ct => ct.ID == id);

            var floor = taskCompleted.CleaningTask.Area.CleaningPlan.HasFloors ? ("på " + taskCompleted.CleaningTask.Floor.Description) : "";

            return $"{cleaningPlanString(taskCompleted.CleaningTask.Area.CleaningPlanID)} opgaven {floor}, område {taskCompleted.CleaningTask.Area.Description} og med beskrivelsen: {taskCompleted.CleaningTask.Comment} er blevet udført med kommentaren: {taskCompleted.Comment}";
        }
    }
}
