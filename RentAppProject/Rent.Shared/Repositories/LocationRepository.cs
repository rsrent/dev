using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentApp;
using RentAppProject;

namespace RentApp
{
    public class LocationRepository
    {
        private readonly HttpClientProvider _clientProvider;
		private readonly IErrorMessageHandler _errorHandler;

		public LocationRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
		{
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
		}

        public async Task Get(int locationID, Action<Location> success, Action error = null) =>
            await _clientProvider.Client.Get("/api/Location/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task<List<Location>> GetAll(Action<List<Location>> success, Action error = null) =>
            await _clientProvider.Client.Get("/api/Location/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task<List<Location>> GetForUser(User user, Action<List<Location>> success, Action error = null) => 
			await _clientProvider.Client.Get("/api/Location/GetForUser/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task GetForCustomer(Customer customer, Action<List<Location>> success, Action error = null) => 
            await _clientProvider.Client.Get("/api/Location/GetForCustomer/" + customer.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task AddLocation(Location newLocation, Customer customer, Action<Location> success, Action error = null) => 
            await _clientProvider.Client.Post("/api/Location/AddLocation/" + customer.ID, newLocation, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

		public async Task UpdateLocation(Location newLocation, Action success, Action error = null) => 
			await _clientProvider.Client.Put("/api/Location/", newLocation, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task AddUser (int locationID, int userID, Action success, Action error = null) => 
            await _clientProvider.Client.Put("/api/Location/AddUser/" + locationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task AddUserToLocations(int userID, ICollection<int> locationIDs, Action success, Action error = null) =>
        await _clientProvider.Client.Put("/api/Location/AddUserToLocations/" + userID, content: locationIDs, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task UpdateLocationUserTitle(int locationID, int userID, string title, Action success, Action error = null) 
        => await _clientProvider.Client.Put("/api/Location/UpdateLocationUserTitle/" + locationID + "/" + userID + "/" + title, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task UpdateLocationUserHourtext(int locationID, int userID, string hourText, Action success, Action error = null) 
        => await _clientProvider.Client.Put("/api/Location/UpdateLocationUserHourtext/" + locationID + "/" + userID + "/" + hourText, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

		public async Task RemoveUser(int locationID, int userID, Action success, Action error = null) => 
            await _clientProvider.Client.Put("/api/Location/RemoveUser/" + locationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task RemoveUserFromLocations(int userID, ICollection<int> locationIDs, Action success, Action error = null) =>
            await _clientProvider.Client.Put("/api/Location/RemoveUserFromLocations/" + userID, content: locationIDs, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task MailToOffice(Location location, string message, Action success, Action error = null) => 
            await _clientProvider.Client.Post("/api/Email/MailToOffice", new { LocationID = location.ID, Message = message}, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Disable(int locationID, Action success, Action error = null) =>
            await _clientProvider.Client.Delete("/api/Location/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task Enable(int locationID, Action success, Action error = null) =>
            await _clientProvider.Client.Put("/api/Location/Enable/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task<List<Location>> GetUsersLocations(User user, Action<List<Location>> success, Action error = null) =>
        await _clientProvider.Client.Get("/api/Location/GetUsersLocations/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task<List<Location>> GetNotUsersLocations(User user, Action<List<Location>> success, Action error = null) =>
        await _clientProvider.Client.Get("/api/Location/GetNotUsersLocations/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
	}
}
