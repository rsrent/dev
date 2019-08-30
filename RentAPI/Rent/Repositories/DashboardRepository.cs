using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices.ComTypes;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.Data;
using Rent.Models;
using Nager.Date;
using Rent.Models.Important;
using Rent.Helpers;

namespace Rent.Repositories
{
    public class DashboardRepository
    {
        private readonly RentContext _rentContext;
        private readonly PermissionRepository _permissionRepository;
        private readonly EconomyRepository _economyRepository;

        private readonly DateTime _largerThanThisDateIsOkay;
        private readonly DateTime _smallerThanThisDateIsLate;


        public DashboardRepository(RentContext rentContext, PermissionRepository permissionRepository, EconomyRepository economyRepository)
        {
            _rentContext = rentContext;
            _permissionRepository = permissionRepository;
            _economyRepository = economyRepository;

            var now = DateTimeHelpers.GmtPlusOneDateTime();

            _largerThanThisDateIsOkay = now.Date.AddDays(2);
            _smallerThanThisDateIsLate = now.Date;
        }

        private void Authorized(int requester)
        {
            if(_permissionRepository.Unauthorized(requester, "Economy", CRUDD.Read))
            {
                throw new UnauthorizedAccessException();
            }
        }

        private dynamic GetData(int requester, Expression<Func<Location, bool>> where)
        {
            Authorized(requester);
            var locations = _rentContext.Location.Where(where).Where(l => !l.Disabled && !l.Customer.Disabled);

            //var locationEconomys = _rentContext.LocationEconomy.Where((e) => locations.Any(l => l.ID == e.LocationID)).ToList();
            var locationEconomys = locations.Select(l => l.LocationEconomy).ToList();
            var locationHours = locations.Select(l => l.LocationHour).ToList();

            //var today = DateTime.Now.Date;
            //var yesterday = DateTime.Now.AddDays(-1).Date;
            var tasks = locations.SelectMany(l => l.CleaningTasks).Where(ct => ct.TimesOfYear != null);

            //var serviceLeaders = _rentContext.User.Where(u => u.RoleID == 3);

            return new
            {
                LocationsTotal = locations.Count(),
                LocationsWithoutStaff = locations.Count(l => l.LocationUsers.All(lu => lu.User.RoleID != 5)),
                LocationsWithoutTasks = locations.Count(l => !l.CleaningTasks.Any()),
                LocationsWithoutServiceLeader = locations.Count(l => l.LocationUsers.All(lu1 => lu1.User.RoleID != 3)),

                IncompleteReports = locations.SelectMany(l => l.QualityReports).Count(q => !q.CompletedTime.HasValue),
                IncompleteMorework = locations.SelectMany(l => l.MoreWork).Count(q => !q.Hours.HasValue),

                ReportsYearly = locations.Sum(l => l.IntervalOfServiceLeaderMeeting != 0 ? 365.0 / l.IntervalOfServiceLeaderMeeting : 0),
                TasksTotal = tasks.Count(),

                //Yesterday = WorkDoneAtDate(locations, yesterday),
                //Today = WorkDoneAtDate(locations, today),
                AllTasks = TaskStatus(tasks),
                WindowTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 2)),
                FanCoilTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 3)),
                PeriodicTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 4)),
                QualityReportStatus = ReportStatus(locations),
                Dg = _economyRepository.CalculateDg(locationEconomys, locationHours),
                ServiceLeaderGeoPositions = locations.Where(l => l.LocationUsers.Any(lu => lu.User.RoleID == 3)).Select(l => new {l.Lat, l.Lon, ServiceLeaderId = l.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 3).UserID, Name = l.Name + ", " + l.Customer.Name}),
                RestGeoPositions = locations.Where(l => !l.LocationUsers.Any(lu => lu.User.RoleID == 3)).Select(l => new {l.Lat, l.Lon, ServiceLeaderId = default(int?), Name = l.Name + ", " + l.Customer.Name}),
            };
        }

        private Dictionary<DateTime, int[]> GetHistory(int requester, Expression<Func<Location, bool>> where, DateTime from, int daysBack)
        {
            Authorized(requester);
            var locations = _rentContext.Location.Where(where);

            Dictionary<DateTime, int[]> dic = new Dictionary<DateTime, int[]>();
            var date = from.Date;
            var counter = 0;
            while (counter <= daysBack){
                dic.Add(date, new int[5]);

                var workDone = WorkDoneAtDate(locations, date);

                dic[date][0] = workDone.Logs;
                dic[date][1] = workDone.Reports;
                dic[date][2] = workDone.Windows;
                dic[date][3] = workDone.Fancoils;
                dic[date][4] = workDone.Periodics;

                date = date.AddDays(-1).Date;
                counter++;
            }

            return dic;
        }
        
        public IEnumerable<dynamic> Customers(int requester)
        {
            Authorized(requester);
            
            //var today = DateTime.Now.Date.AddDays(2);
            //var yesterday = DateTime.Now.AddDays(-1).Date;
            
            return _rentContext
                .Customer
                .Where(c => !c.Disabled)
                .Select(c => new
                {
                    c.ID,
                    c.Name,
                    
                    LocationsTotal = c.Locations.Count(l => !l.Disabled),
                    LocationsWithoutTasks = c.Locations.Where(l => !l.Disabled).Count(l => !l.CleaningTasks.Any()),
                    LocationsWithoutStaff = c.Locations.Where(l => !l.Disabled).Count(l => l.LocationUsers.All(lu1 => lu1.User.RoleID != 5)),
                    LocationsWithoutServiceLeader = c.Locations.Where(l => !l.Disabled).Count(l => l.LocationUsers.All(lu1 => lu1.User.RoleID != 3)),
                    
                    IncompleteReports = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.QualityReports).Count(q => !q.CompletedTime.HasValue),
                    IncompleteMorework = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.MoreWork).Count(m => m.Hours == null),
                    ReportsYearly = c.Locations.Where(l => !l.Disabled).Sum(l => l.IntervalOfServiceLeaderMeeting != 0 ? 365.0 / l.IntervalOfServiceLeaderMeeting : 0),
                    TasksTotal = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.CleaningTasks).Count(t => t.TimesOfYear != null),
                    TasksDelayed = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.CleaningTasks).Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                        t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) < _smallerThanThisDateIsLate),
                    TasksOkay = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.CleaningTasks).Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                        t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) > _largerThanThisDateIsOkay),
                    TasksUnstarted = c.Locations.Where(l => !l.Disabled).SelectMany(l => l.CleaningTasks).Count(t => t.TimesOfYear != null && t.FirstCleaned == null),
                    
                    ReportsDelayed = c.Locations.Where(l => !l.Disabled).Where(t => t.FirstQualityReportTime != null).Count(t =>
                        t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) < _smallerThanThisDateIsLate),
            
                    ReportsOkay = c.Locations.Where(l => !l.Disabled).Where(t => t.FirstQualityReportTime != null).Count(t =>
                    t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) > _largerThanThisDateIsOkay),
            
                    ReportsUnstarted = c.Locations.Where(l => !l.Disabled).Count(t => t.FirstQualityReportTime == null),

                    Dg = _economyRepository.CalculateDg(c.Locations.Where(l => !l.Disabled).Select(l => l.LocationEconomy).ToList(), c.Locations.Where(l => !l.Disabled).Select(l => l.LocationHour).ToList()),
                });
        }

        public IEnumerable<dynamic> Users(int requester)
        {
            Authorized(requester);
            
            //var today = DateTime.Now.Date.AddDays(2);
            //var yesterday = DateTime.Now.AddDays(-1).Date;

            return _rentContext.User.Where(u => u.RoleID == 3 && !u.Disabled).Select(u => new
            {
                u.ID,
                Name = u.FirstName + " " + u.LastName,

                LocationsTotal = u.LocationUsers.Count(),
                LocationsWithoutStaff = u.LocationUsers.Count(lu => lu.Location.LocationUsers.All(lu1 => lu1.User.RoleID != 5)),
                LocationsWithoutTasks = u.LocationUsers.Count(lu => !lu.Location.CleaningTasks.Any()),
                LocationsWithoutServiceLeader = 0,
                    
                IncompleteReports = u.LocationUsers.Select(lu => lu.Location).SelectMany(l => l.QualityReports).Count(q => !q.CompletedTime.HasValue),
                IncompleteMorework = u.LocationUsers.Select(lu => lu.Location).SelectMany(l => l.MoreWork).Count(m => m.Hours == null),
                ReportsYearly = u.LocationUsers.Sum(lu => lu.Location.IntervalOfServiceLeaderMeeting != 0 ? 365.0 / lu.Location.IntervalOfServiceLeaderMeeting : 0),
                TasksTotal = u.LocationUsers.SelectMany(lu => lu.Location.CleaningTasks).Count(t => t.TimesOfYear != null),
                TasksDelayed = u.LocationUsers.SelectMany(lu => lu.Location.CleaningTasks).Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                    t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) < _smallerThanThisDateIsLate),
                TasksOkay = u.LocationUsers.SelectMany(lu => lu.Location.CleaningTasks).Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                    t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) > _largerThanThisDateIsOkay),
                TasksUnstarted = u.LocationUsers.SelectMany(lu => lu.Location.CleaningTasks).Count(t => t.TimesOfYear != null && t.FirstCleaned == null),
                
                ReportsDelayed = u.LocationUsers.Select(lu => lu.Location).Count(t => t.FirstQualityReportTime != null &&
                                                                                 t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) < _smallerThanThisDateIsLate),
                
                ReportsOkay = u.LocationUsers.Select(lu => lu.Location).Count(t => t.FirstQualityReportTime != null &&
                                                                              t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) > _largerThanThisDateIsOkay),
            
                ReportsUnstarted = u.LocationUsers.Select(lu => lu.Location).Count(t => t.FirstQualityReportTime == null),

                Dg = _economyRepository.CalculateDg(u.LocationUsers.Select(lu => lu.Location.LocationEconomy).ToList(), u.LocationUsers.Select(lu => lu.Location.LocationHour).ToList()),
            });
        }

        private IEnumerable<dynamic> GetLocations(int requester, Expression<Func<Location, bool>> where)
        {
            Authorized(requester);
            
            //var whenBeforeIsOkayDate = DateTime.Now.Date.AddDays(2);
            //var yesterday = DateTime.Now.AddDays(-1).Date;

            return _rentContext

                .Location
                .Where(where)
                .Where(l => !l.Disabled && !l.Customer.Disabled)
                .Select(l => new
                {
                    l.ID,
                    l.Name,
                    CustomerName = l.Customer.Name,
                    ServiceLeaderName = l.LocationUsers.Any(lu => lu.User.RoleID == 3) ? l.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 3).User.FirstName + " " + l.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 3).User.LastName : default(string),


                    NextQualityReport = l.FirstQualityReportTime != null ?
                        l.FirstQualityReportTime.Value
                                         .AddDays(l.IntervalOfServiceLeaderMeeting * l.NumberOfQualityReportsSinceUpdate)
                            .ToString("yyyy-MM-dd HH:mm:ss")
                        : default(string),

                    AgreedNextQualityReport = l.LatestQualityReport.NextReport.HasValue ? l.LatestQualityReport.NextReport.Value.ToString("yyyy-MM-dd HH:mm:ss") : default(string),

                LocationsTotal = 1,
                LocationsWithoutStaff = l.LocationUsers.Any(lu => lu.User.RoleID == 5) ? 0 : 1,
                LocationsWithoutTasks = l.CleaningTasks.Any() ? 0 : 1,
                LocationsWithoutServiceLeader = l.LocationUsers.Any(lu => lu.User.RoleID == 3) ? 0 : 1,

                IncompleteReports = l.QualityReports.Count(q => !q.CompletedTime.HasValue),
                IncompleteMorework = l.MoreWork.Count(m => m.Hours == null),

                ReportsYearly = l.IntervalOfServiceLeaderMeeting != 0 ? 365.0 / l.IntervalOfServiceLeaderMeeting : 0,
                TasksTotal = l.CleaningTasks.Count(t => t.TimesOfYear != null),
                    TasksDelayed = l.CleaningTasks.Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                            t.FirstCleaned.Value.AddDays((int)(365.0 / t.TimesOfYear) * t.CompletedTasks.Count) < _smallerThanThisDateIsLate),
                    TasksOkay = l.CleaningTasks.Where(t => t.TimesOfYear != null && t.FirstCleaned != null).Count(t =>
                            t.FirstCleaned.Value.AddDays((int)(365.0 / t.TimesOfYear) * t.CompletedTasks.Count) > _largerThanThisDateIsOkay),
                    TasksUnstarted = l.CleaningTasks.Count(t => t.TimesOfYear != null && t.FirstCleaned == null),

                    ReportsDelayed = 0,
                    ReportsOkay = 0,
                    ReportsUnstarted = 0,

                Dg = _economyRepository.CalculateDg(new [] {l.LocationEconomy}.ToList(), new[] { l.LocationHour }.ToList()),

                });
        }
        
        private IEnumerable<dynamic> GetTasks(int requester, Expression<Func<CleaningTask,bool>> where)
        {
            Authorized(requester);

            return _rentContext
                .CleaningTask
                .Where(where)
                .Where(t => t.Active && !t.Location.Disabled && !t.Location.Customer.Disabled)
                .Include(c => c.LastTaskCompleted)
                .Include(c => c.Location)
                .ThenInclude(l => l.Customer)
                .Include(c => c.Location)
                .ThenInclude(l => l.LocationUsers)
                .ThenInclude(lu => lu.User)
                .Include(c => c.Location)
                .ThenInclude(l => l.LatestQualityReport)
                .Include(ct => ct.CompletedTasks)
                .ThenInclude(cct => cct.CompletedByUser)
                .Include(ct => ct.Area)
                .ThenInclude(a => a.CleaningPlan)
                .Include(ct => ct.Floor)
                .Where(ct => ct.TimesOfYear != null)
                .ToList()
                .Select(ct => new
                {
                    PlanId = ct.Area.CleaningPlanID,
                    Plan = ct.Area.CleaningPlan.Description,
                    Area = ct.Area.Description,
                    Floor = ct.Floor != null ? ct.Floor.Description : "",
                    Comment = ct.Comment,
                    ct.TimesOfYear,
                    LastCleaned = ct.LastTaskCompleted?.CompletedDate.ToString("yyyy-MM-dd HH:mm:ss"),
                    LocationName = ct.Location.Name,
                    CustomerName = ct.Location.Customer.Name,
                });
        }

        private IEnumerable<dynamic> GetLogs(int requester, Expression<Func<LocationLog,bool>> where)
        {
            Authorized(requester);
                
            return _rentContext
                .LocationLog
                .Where(where)
                .Include(l => l.User)
                .Include(l => l.Location)
                .ThenInclude(l => l.Customer)
                .OrderByDescending(l => l.DateCreated)
                .Select(l => new
                {
                    l.ID,
                    l.Title,
                    l.Log,
                    UserName = l.User != null ? l.User.FirstName + " " + l.User.LastName : default(string),
                    LocationName = l.Location.Name,
                    CustomerName = l.Location.Customer.Name,
                    Time = l.DateCreated.ToString("yyyy-MM-dd HH:mm:ss")
                });
        }

        private IEnumerable<dynamic> GetMoreWork(int requester, Expression<Func<MoreWork,bool>> where)
        {
            Authorized(requester);

            return _rentContext
                .MoreWork
                .Where(where)
                .Where(m => m.Hours == null)
                .Select(m => new
                {
                    m.Location.Name,
                    m.ExpectedCompletedTime,
                });
        }

        private IEnumerable<dynamic> GetNews(int requester, Expression<Func<News, bool>> where)
        {
            Authorized(requester);

            return _rentContext
                .News.OrderByDescending(n => n.Time).Take(20)
                .Where(where).Select(n => new
                {
                    n.Id,
                    n.Title,
                    n.Body,
                    Time = n.Time.ToString("yyyy-MM-dd HH:mm:ss"),
                });
        }
        
        /// <summary>
        /// Return data for the relevant locations
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <param name="locationId"></param>
        /// <returns></returns>
        public dynamic Data(int requester, int customerId, int userId, int locationId)
        {
            return GetData(requester, l => (customerId == 0 || l.CustomerID == customerId)
                                        && (userId == 0 || l.LocationUsers.Any(lu => lu.UserID == userId)) 
                                        && (locationId == 0 || l.ID == locationId));
        }

        /// <summary>
        /// Gets work history between two dates
        /// </summary>
        /// <returns>The data.</returns>
        /// <param name="requester">Requester.</param>
        /// <param name="customerId">Customer identifier.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="locationId">Location identifier.</param>
        public dynamic WorkHistory(int requester, int customerId, int userId, int locationId, int daysBack, DateTime from)
        {
            return GetHistory(requester, l => (customerId == 0 || l.CustomerID == customerId)
                                        && (userId == 0 || l.LocationUsers.Any(lu => lu.UserID == userId))
                                        && (locationId == 0 || l.ID == locationId), from, daysBack);
        }
        
        /// <summary>
        /// Returns locations
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Locations(int requester, int customerId, int userId)
        {
            return GetLocations(requester, l => (customerId == 0 || l.CustomerID == customerId)
                                             && (userId == 0 || l.LocationUsers.Any(lu => lu.UserID == userId)));
        }
        
        /// <summary>
        /// Returns locations without a plan
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LocationsWithoutPlan(int requester, int customerId, int userId)
        {
            return GetLocations(requester, l => !l.CleaningTasks.Any() 
                                             && (customerId == 0 || l.CustomerID == customerId)
                                             && (userId == 0 || l.LocationUsers.Any(lu => lu.UserID == userId)));
        }
        
        /// <summary>
        /// Returns locations without staff
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LocationsWithoutStaff(int requester, int customerId, int userId)
        {
            return GetLocations(requester, l => l.LocationUsers.All(lu => lu.User.RoleID != 5)
                                             && (customerId == 0 || l.CustomerID == customerId)
                                             && (userId == 0 || l.LocationUsers.Any(lu => lu.UserID == userId)));
        }
        
        /// <summary>
        /// Retruns locations without a service leader
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> LocationsWithoutServiceLeader(int requester, int customerId)
        {
            return GetLocations(requester, l => l.LocationUsers.All(lu => lu.User.RoleID != 3)
                                             && (customerId == 0 || l.CustomerID == customerId));
        }
        
        /// <summary>
        /// Returns tasks
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="locationId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Tasks(int requester, int locationId, int customerId, int userId)
        {
            return GetTasks(requester, ct => (locationId == 0 || ct.LocationID == locationId)
                                             && (customerId == 0 || ct.Location.CustomerID == customerId)
                                             && (userId == 0 || ct.Location.LocationUsers.Any(lu => lu.UserID == userId)));
        }
        
        /// <summary>
        /// Returns logs
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="locationId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> Logs(int requester, int locationId, int customerId, int userId)
        {
            return GetLogs(requester, l => (locationId == 0 || l.LocationID == locationId)
                           && (customerId == 0 || l.Location.CustomerID == customerId)
                           && (userId == 0 || l.UserID == userId));
        }

        /// <summary>
        /// Return morework
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="locationId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> MoreWork(int requester, int locationId, int customerId, int userId)
        {
            return GetMoreWork(requester, m => (locationId == 0 || m.LocationID == locationId)
                               && (customerId == 0 || m.Location.CustomerID == customerId)
                               && (userId == 0 || m.Location.LocationUsers.Any(lu => lu.UserID == userId)));
        }

        /// <summary>
        /// Return morework
        /// </summary>
        /// <param name="requester"></param>
        /// <param name="locationId"></param>
        /// <param name="customerId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public IEnumerable<dynamic> News(int requester, int locationId, int customerId, int userId)
        {
            return GetNews(requester, m => (locationId == 0 || m.LocationId == locationId)
                               && (customerId == 0 || m.Location.CustomerID == customerId)
                               && (userId == 0 || m.Location.LocationUsers.Any(lu => lu.UserID == userId)));
        }

        public float GetDg(int requester, int locationId, int customerId, int userId)
        {
            Authorized(requester);

            if (locationId != 0) return _economyRepository.GetDgForLocation(locationId);
            else if (customerId != 0) return _economyRepository.GetDgForCustomer(customerId);
            else if (userId != 0) return _economyRepository.GetDgForUser(userId);
            else return _economyRepository.GetDgForAll();
        }

        public List<DateTime> getHolidays()
        {
            CultureInfo.CurrentCulture = new CultureInfo("da-DK");

            var holidays = DateSystem.GetPublicHoliday("DK", DateTime.Now.Year).Select(h => h.Date).ToList();

            var palmSunday = holidays[3].AddDays(-7);
            var christmasEve = holidays[9].AddDays(-1);
            var newYearseve = new DateTime(DateTime.Now.Year, 12, 31);

            holidays.Insert(3, palmSunday);
            holidays.Insert(10, christmasEve);
            holidays.Insert(13, newYearseve);
            return holidays;
        }
        /*
        dynamic RecentNumbers(IQueryable<Location> locations)
        {
            var today = DateTime.Now.Date;
            var yesterday = DateTime.Now.AddDays(-1).Date;
            var tasks = locations.SelectMany(l => l.CleaningTasks).Where(ct => ct.TimesOfYear != null);
            
            return new
            {
                LocationsTotal = locations.Count(),
                LocationsWithoutStaff = locations.Count(l => l.LocationUsers.All(lu => lu.User.RoleID != 5)),
                LocationsWithoutTasks = locations.Count(l => !l.CleaningTasks.Any()),
                LocationsWithoutServiceLeader = locations.Count(l => l.LocationUsers.All(lu1 => lu1.User.RoleID != 3)),
                
                IncompleteReports = locations.SelectMany(l => l.QualityReports).Count(q => !q.CompletedTime.HasValue),
                IncompleteMorework = locations.SelectMany(l => l.MoreWork).Count(q => !q.Hours.HasValue),

                ReportsYearly = locations.Sum(l => l.IntervalOfServiceLeaderMeeting != 0 ? 365.0 / l.IntervalOfServiceLeaderMeeting : 0),
                TasksTotal = tasks.Count(),
                
                Yesterday = WorkDoneAtDate(locations, yesterday),
                Today = WorkDoneAtDate(locations, today),
                AllTasks = TaskStatus(tasks),
                WindowTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 2)),
                FanCoilTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 3)),
                PeriodicTasks = TaskStatus(tasks.Where(t => t.Area.CleaningPlanID == 4)),
                QualityReportStatus = ReportStatus(locations),
            };
        } */

        dynamic TaskStatus(IQueryable<CleaningTask> tasks)
        {
            //var today = DateTime.Now.Date.AddDays(2);
            //var yesterday = DateTime.Now.AddDays(-1).Date;
            
            var array = new int [4];
            array[0] =  tasks
                .Where(t => t.FirstCleaned != null).Count(t =>
                    t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) < _smallerThanThisDateIsLate);
            
            array[2] = tasks
                .Where(t => t.FirstCleaned != null).Count(t =>
                    t.FirstCleaned.Value.AddDays((int) (365.0 / t.TimesOfYear) * t.CompletedTasks.Count) > _largerThanThisDateIsOkay);
            
            array[3] = tasks
                .Count(t => t.FirstCleaned == null);

            array[1] = tasks.Count() - array[0] - array[2] - array[3];
            return new
            {
                Delayed = array[0], Critical = array[1], Okay = array[2], Unstarted = array[3]
            };
        }
        
        dynamic ReportStatus(IQueryable<Location> locations)
        {
            //var today = DateTime.Now.Date.AddDays(2);
            //var yesterday = DateTime.Now.AddDays(-1).Date;
            
            var array = new int [4];
            array[0] = locations.Where(t => t.FirstQualityReportTime != null).Count(t =>
                                                                                    t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) < _smallerThanThisDateIsLate);
            
            array[2] = locations.Where(t => t.FirstQualityReportTime != null).Count(t =>
                                                                                    t.FirstQualityReportTime.Value.AddDays(t.IntervalOfServiceLeaderMeeting * t.NumberOfQualityReportsSinceUpdate) > _largerThanThisDateIsOkay);
            
            array[3] = locations.Count(t => t.FirstQualityReportTime == null);

            array[1] = locations.Count() - array[0] - array[2] - array[3];
            return new
            {
                Delayed = array[0], Critical = array[1], Okay = array[2], Unstarted = array[3]
            };
        }

        dynamic WorkDoneAtDate(IQueryable<Location> locations, DateTime date)
        {
            var tasks = locations.SelectMany(l => l.CleaningTasks).Where(ct => ct.TimesOfYear != null);
            var completedTasks = tasks.SelectMany(t => t.CompletedTasks);
            return new
            {
                Logs = locations.SelectMany(l => l.LocationLogs)
                    .Count(log => log.DateCreated.Date.Equals(date)),
                Reports = locations.SelectMany(l => l.QualityReports)
                    .Count(rep => rep.Time.Date.Equals(date)),
                
                Windows = completedTasks.Count(ct =>
                    ct.CleaningTask.Area.CleaningPlanID == 2 && ct.CompletedDate.Date.Equals(date)),
                Fancoils = completedTasks.Count(ct =>
                    ct.CleaningTask.Area.CleaningPlanID == 3 && ct.CompletedDate.Date.Equals(date)),
                Periodics = completedTasks.Count(ct =>
                    ct.CleaningTask.Area.CleaningPlanID == 4 && ct.CompletedDate.Date.Equals(date)),
            };
        }
    }
}