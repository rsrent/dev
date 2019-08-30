using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.ContextPoint.Exceptions;
using Rent.DTOs;
using Rent.Helpers;
using Rent.Models;

namespace Rent.Repositories
{
    public class QualityReportRepository
    {
        private readonly QualityReportContext _qualityReportContext;
        private readonly QualityReportItemContext _qualityReportItemContext;
        private readonly CleaningTaskContext _cleaningTaskContext;
        private readonly LocationContext _locationRepository;
        private readonly NotificationRepository _notificationRepository;
        private readonly RatingRepository _ratingRepository;
        private readonly PropCondition _propCondition;
        private readonly NewsRepository _newsRepository;
        private readonly SendGridEmail _sendGridEmail;

        public QualityReportRepository(QualityReportContext qualityReportContext, QualityReportItemContext qualityReportItemContext, CleaningTaskContext cleaningTaskContext, LocationContext locationRepository, NotificationRepository notificationRepository, RatingRepository ratingRepository, PropCondition propCondition, NewsRepository newsRepository, SendGridEmail sendGridEmail)
        {
            _qualityReportContext = qualityReportContext;
            _qualityReportItemContext = qualityReportItemContext;
            _cleaningTaskContext = cleaningTaskContext;
            _locationRepository = locationRepository;
            _notificationRepository = notificationRepository;
            _ratingRepository = ratingRepository;
            _propCondition = propCondition;
            _newsRepository = newsRepository;
            _sendGridEmail = sendGridEmail;
        }

        public dynamic GetPlanWithFloors(int requester, int id)
        {
            var qrp = _qualityReportContext
                .DatabaseOne(requester,
                        q => q.ID == id,
                        "QualityReportItems.CleaningTask.Area",
                        "QualityReportItems.CleaningTask.Floor");

            return new
            {
                qrp.ID, qrp.LocationID, qrp.CompletedTime, qrp.Time, qrp.UserID, qrp.Comment,
                Floors = qrp.QualityReportItems.GroupBy(t => t.CleaningTask.Floor).Select(g1 =>
                    new
                    {
                        Floor = g1.Key,
                        Areas = g1.GroupBy(a => a.CleaningTask.Area).Select(g2 => new
                        {
                            Area = g2.Key,
                            QualityReportItems = g2.Select(q => q.Basic()).ToList()
                        }).OrderBy(a => a.Area.ID)
                    }).OrderBy(f => f.Floor.ID)
            };
        }

        public dynamic GetQualityReportsForLocation(int requester, int locationID)
        {
            return _qualityReportContext
                .BasicOrdered(requester, q => q.LocationID == locationID, query => query.OrderByDescending(q => q.ID),
                    "QualityReportItems.CleaningTask");
        }

        public async Task UpdateCustomerComment(int requester, int reportId, string comment)
        {
            await _qualityReportContext.Update(0, i => i.ID == reportId, i =>
            {
                i.Comment = comment.Substring(1, comment.Length - 2);
            });

            var report = _qualityReportContext.DatabaseOne(0, q => q.ID == reportId);

            var header = "Ny kommentar til kvalitetsrapport";
            var body = report.Comment;
            await _newsRepository.AddNews(requester, report.LocationID, header, body);

            var location = _locationRepository.DatabaseOne(0, l => l.ID == report.LocationID, "LocationUsers.User");

            if(location != null)
            {
                var locationUser = location.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 3);
                if(locationUser != null && locationUser.User.Email != null)
                {
                    try {
                        await _sendGridEmail.Send(locationUser.User.Email, header, body);
                    } catch(Exception e) {}
                }
            }

        }

        public async Task AddItem(int requester, int qualityReportId, QualityReportItem qualityReportItem)
        {
            try {
                var oldItem = _qualityReportItemContext.GetOne(requester, i => i.CleaningTaskID == qualityReportItem.CleaningTaskID && i.QualityReportID == qualityReportId);
                qualityReportItem.ID = oldItem.ID;
                await UpdateItem(requester, qualityReportItem);
            } catch (NotFoundException e) {
                qualityReportItem.QualityReportID = qualityReportId;
                var res = await _qualityReportItemContext.Create(requester, qualityReportItem);
            }
        }

        public async Task UpdateItem (int requester, QualityReportItem qualityReportItem)
        {
            await _qualityReportItemContext.Update(requester, i => i.ID == qualityReportItem.ID, i =>
            {
                i.Comment = qualityReportItem.Comment;
                i.Rating = qualityReportItem.Rating;
                i.ImageLocation = qualityReportItem.ImageLocation;
            });
        }

        public async Task<dynamic> CreateReport(int requester, int locationID)
        {
            var qualityReport = new QualityReport { Time = DateTimeHelpers.GmtPlusOneDateTime(), LocationID = locationID, UserID = requester };
            var created = await _qualityReportContext.Create(requester, qualityReport);

            var qualityReportItems = _cleaningTaskContext.Database(requester, c => c.Active && c.LocationID == locationID && c.Area.CleaningPlanID == 1).Select(c => new QualityReportItem
            {
                CleaningTaskID = c.ID,
                QualityReportID = qualityReport.ID
            }).ToList();
            await _qualityReportItemContext.Create(requester, qualityReportItems);

            await _newsRepository.AddNews(requester, locationID, Models.Important.NewsCategory.QualityReportStarted, created.ID);

            return created.Basic();
        }

        public async Task CompleteReport(int requester, int qualityReportId, DateTime nextMeetingDate, int ratingValue)
        {
            var rating = await _ratingRepository.CreateRating("Kvalitetsrapport " + DateTimeHelpers.GmtPlusOneDateTime().ToString("d"));

            var report = _qualityReportContext.DatabaseOne(0, q => q.ID == qualityReportId);

            var location = _locationRepository.DatabaseOne(0, l => l.ID == report.LocationID);

            await _ratingRepository.CreateRatingItem(rating.ID, new RatingItem
            {
                RatedByID = requester,
                TimeRated = DateTimeHelpers.GmtPlusOneDateTime(),
                Value = ratingValue,
                Title = "Vurdering af samarbejdet"
            });
            await _qualityReportContext.Update(requester, q => q.ID == qualityReportId, q =>
            {
                q.CompletedTime = DateTimeHelpers.GmtPlusOneDateTime();
                q.RatingID = rating.ID;
                q.NextReport = nextMeetingDate;
            });

            await _locationRepository.Update(0, l => l.ID == report.LocationID, (l) =>
            {
                l.NumberOfQualityReportsSinceUpdate++;
                l.LatestQualityReportID = qualityReportId;

                if(l.FirstQualityReportTime == null)
                {
                    l.FirstQualityReportTime = DateTimeHelpers.GmtPlusOneDateTime();
                }
            });



            //var report = _qualityReportContext.DatabaseOne(0, q => q.ID == qualityReportId);

            _notificationRepository.QualityReportCompleted(qualityReportId, nextMeetingDate, ratingValue);

            await _newsRepository.AddNews(requester, report.LocationID, Models.Important.NewsCategory.QualityReportCompleted, report.ID);
        }

        public async Task DeleteQualityReport(int requester, int qualityReportId)
        {
            await _qualityReportContext.Delete(requester, q => q.ID == qualityReportId);
        }

        public IEnumerable<dynamic> Test(int requester, DateTime untill)
        {
            return _locationRepository.Basic(requester,
                l => l.QualityReports.Any() &&
                     l.QualityReports.Last().Time + new TimeSpan(l.IntervalOfServiceLeaderMeeting, 0, 0, 0) < untill);
            //.ToList().Select()
            //.Where(ct => ct.NextQualityReport < untill)
            //.OrderBy(ct => ct.NextQualityReport);
        }
    }
}
