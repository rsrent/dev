using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rent.Data;
using Rent.Models;
using Microsoft.EntityFrameworkCore;
using Rent.DTOs;
using Rent.ContextPoint;

namespace Rent.Repositories
{
    public class LocationRepository
    {
        private readonly RentContext _context;
        private readonly LocationContext _locationContext;
        private readonly UserContext _userContext;
        private readonly LocationUserContext _locationUserContext;

        private readonly DocumentRepository _documentRepository;
        private readonly PropCondition _propCondition;

        public LocationRepository(RentContext context, LocationContext locationContext, UserContext userContext, LocationUserContext locationUserContext, DocumentRepository documentRepository, PropCondition propCondition)
        {
            _context = context;
            _locationContext = locationContext;
            _userContext = userContext;
            _locationUserContext = locationUserContext;

            _documentRepository = documentRepository;
            _propCondition = propCondition;
        }

        public async Task<dynamic> Add(int requester, int customerId, Location location)
        {
            var newLocation = new Location
            {
                Name = location.Name,
                Address = location.Address,
                Comment = location.Comment,
                CustomerID = customerId,
                ProjectNumber = location.ProjectNumber,
                Phone = location.Phone,
                IntervalOfServiceLeaderMeeting = location.IntervalOfServiceLeaderMeeting,
                GeneralFolderID = await _documentRepository.GetLocationGeneralFolder(requester),
                CleaningplanFolderID = await _documentRepository.GetLocationCleaningPlanFolder(requester),
                CleaningFolderID = await _documentRepository.GetStandardCleaningFolder(requester, "Rengøring"),
                WindowFolderID = await _documentRepository.GetStandardCleaningFolder(requester, "Vinduespudsning"),
                FanCoilFolderID = await _documentRepository.GetStandardCleaningFolder(requester, "Fan coil"),
                PeriodicFolderID = await _documentRepository.GetStandardCleaningFolder(requester, "Periodisk rengøring"),
                Lat = location.Lat,
                Lon = location.Lon,
            };

            var conversations = new List<Conversation>
            {
                new Conversation { Title = location.Name + ", Holdet", Open = false },
                new Conversation { Title = location.Name, Open = false }
            };

            _context.Conversation.AddRange(conversations);
            await _context.SaveChangesAsync();

            newLocation.TeamConversationID = conversations[0].ID;
            newLocation.CustomerConversationID = conversations[1].ID;

            var locationHour = new LocationHour { };
            _context.LocationHour.Add(locationHour);

            var locationEconomy = new LocationEconomy { };
            _context.LocationEconomy.Add(locationEconomy);

            await _context.SaveChangesAsync();
            newLocation.LocationHourID = locationHour.ID;
            newLocation.LocationEconomyID = locationEconomy.ID;

            var createdLocation = await _locationContext.Create(requester, newLocation);


            //_context.Location.Add(newLocation);
            //await _context.SaveChangesAsync();





            await _context.SaveChangesAsync();

            return createdLocation.Detailed();
        }

        public async Task AddUserToLocation(int requester, int locationId, int userId)
        {
            var user = _userContext.DatabaseOne(requester, u => u.ID == userId);
            var location = _locationContext.DatabaseOne(requester, l => l.ID == locationId);
            await _locationUserContext.Create(requester, new LocationUser { UserID = user.ID, LocationID = location.ID });
        }

        public async Task AddUserToLocations(int requester, ICollection<int> locationIDs, int userId)
        {
            foreach (var id in locationIDs)
                await AddUserToLocation(requester, id, userId);
        }

        public async Task RemoveUserFromLocations(int requester, ICollection<int> locationIDs, int userId)
        {
            await _locationUserContext.Delete(requester, (lu) => lu.UserID == userId && locationIDs.Contains(lu.LocationID));
        }

        public async Task UpdateLocationUser(int requester, int locationId, int userId, string title = null, string hourText = null)
        {
            await _locationUserContext.Update(requester, lu => lu.LocationID == locationId && lu.UserID == userId, lu =>
            {
                if (title != null) lu.Title = title;
                if (hourText != null) lu.HourText = hourText;
            });
        }

        public dynamic GetLocationInformation(int requester, int locationId)
        {
            return _locationContext.DetailedOne(requester, l => l.ID == locationId, "Customer", "LocationUsers.User");
        }

        public IEnumerable<dynamic> GetAll(int requester)
        {
            return _locationContext.Basic(requester, null, "Customer")
                                   .OrderBy(l => l.CustomerName).ThenBy(l => l.Name);
        }

        public IEnumerable<dynamic> GetLocationsForUser(int requester, int userId)
        {
            return _locationContext.Basic(requester, l => l.LocationUsers.Any(lu => lu.UserID == userId), "Customer", "LocationUsers")
                                   .OrderBy(l => l.CustomerName).ThenBy(l => l.Name);
            /*
            return _locationUserContext.Get(requester, 
                                            lu => lu.UserID == userId || 
                                            lu.Location.Customer.MainUserID == userId || 
                                            lu.Location.Customer.KeyAccountManagerID == userId, 
                                            "Location.Customer")
                                       .Select(lu => lu.ToLocation()).ToList()
                                       .OrderBy(l => l.CustomerName).ThenBy(l => l.Name);*/
        }

        public IEnumerable<dynamic> GetUsersLocations(int requester, int userId)
        {
            return _locationContext.Basic(requester, l => l.LocationUsers.Any(lu => lu.UserID == userId), "Customer", "LocationUsers")
                                   .OrderBy(l => l.CustomerName).ThenBy(l => l.Name);
            /*
            return _locationUserContext.Get(requester, lu => lu.UserID == userId, "Location.Customer").Distinct()
                                       .Select(lu => lu.ToLocation()).ToList()
                                       .OrderBy(l => l.Name); */
        }


        public IEnumerable<dynamic> GetNotUsersLocations(int requester, int userId)
        {
            var userLocations = _locationUserContext.Get(requester, lu => lu.UserID == userId).Select(lu => lu.LocationID).ToList();
            return _locationContext.Basic(requester, l => !userLocations.Contains(l.ID), "Customer")
                                   .OrderBy(l => l.Name);
        }

        public string GetName(int requester, int locationId)
        {
            return _locationContext.BasicOne(requester, l => l.ID == locationId)?.Name;
        }

        public async Task Disable(int requester, int locationId)
        {
            await _locationContext.Update(requester, l => l.ID == locationId, l => { l.Disabled = true; });
        }

        public async Task Enable(int requester, int locationId)
        {
            await _locationContext.Update(requester, l => l.ID == locationId, l => { l.Disabled = false; });
        }

        public async Task Update(int requester, Location ul)
        {
            await _locationContext.Update(requester, l => l.ID == ul.ID, l =>
            {
                l.Comment = ul.Comment;
                l.Address = ul.Address;
                l.Name = ul.Name;
                l.Phone = ul.Phone;
                l.ImageLocation = ul.ImageLocation;
                l.ProjectNumber = ul.ProjectNumber;
                l.IntervalOfServiceLeaderMeeting = ul.IntervalOfServiceLeaderMeeting;

                if(l.IntervalOfServiceLeaderMeeting != ul.IntervalOfServiceLeaderMeeting)
                {
                    if(l.LatestQualityReport != null)
                    {
                        l.NumberOfQualityReportsSinceUpdate = 1;
                        l.FirstQualityReportTime = l.LatestQualityReport.CompletedTime;
                    } else {
                        l.LatestQualityReportID = null;
                        l.NumberOfQualityReportsSinceUpdate = 0;
                        l.FirstQualityReportTime = null;
                    }
                }

                l.Lat = ul.Lat;
                l.Lon = ul.Lon;
            }, "LatestQualityReport");
        }

        public IEnumerable<dynamic> GetLocationsForCustomer(int requester, int customerId)
        {
            return _locationContext.BasicOrdered(requester, l => l.CustomerID == customerId, q => q.OrderBy(l => l.Name), "LocationUsers.User", "LocationHour");
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    _context.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~CharacterRepository() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}