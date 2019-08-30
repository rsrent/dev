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

        public async Task<List<Location>> GetForUser(User user, Action<List<Location>> success, Action error = null) {
			return await _clientProvider.Client.Get("/api/Location/GetForUser/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task GetForCustomer(Customer customer, Action<List<Location>> success, Action error = null) {
            await _clientProvider.Client.Get("/api/Location/GetForCustomer/" + customer.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task AddLocation(Location newLocation, Customer customer, Action<Location> success, Action error = null) {
            await _clientProvider.Client.Post("/api/Location/AddLocation/" + customer.ID, newLocation, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

		public async Task UpdateLocation(Location newLocation, Action success, Action error = null) {
			await _clientProvider.Client.Put("/api/Location/", newLocation, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task AddUser (int locationID, int userID, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Location/AddUser/" + locationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

		public async Task RemoveUser(int locationID, int userID, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Location/RemoveUser/" + locationID + "/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task DeleteLocation(int locationID, Action success, Action error = null) {
            await _clientProvider.Client.Delete("/api/Location/" + locationID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
	}
}
