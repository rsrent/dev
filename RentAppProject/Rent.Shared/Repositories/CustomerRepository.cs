using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentApp;
using RentAppProject;

namespace RentApp
{
    public class CustomerRepository
    {
        private readonly HttpClientProvider _clientProvider;
		private readonly IErrorMessageHandler _errorHandler;

		public CustomerRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
		{
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
		}

        public async Task Get(int customerID, Action<Customer> success, Action error = null) =>
        await _clientProvider.Client.Get("/api/Customers/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

		public async Task GetAll(Action<List<Customer>> success, Action error = null) =>
			await _clientProvider.Client.Get("/api/Customers/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		
        public async Task<Customer> GetForCustomerUser(User user, Action<Customer> success, Action error = null) {
            return await _clientProvider.Client.Get("/api/Customers/GetForCustomerUser/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

		public async Task AddCustomer(Customer newCustomer, Action<Customer> success, Action error = null) {
            await _clientProvider.Client.Post("/api/Customers/AddCustomer/", newCustomer, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

		public async Task UpdateCustomer(Customer newCustomer, Action success, Action error = null) {
            await _clientProvider.Client.Put("/api/Customers/", content: newCustomer, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task Invite(int customerID, Action success, Action error = null)
        => await _clientProvider.Client.Put("/api/Customers/Invite/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

		public async Task AddUser(int customerID, int userID, Action success, Action error = null) {
			await _clientProvider.Client.Put("/api/Customers/AddUser/" + customerID + "/" + userID, success, error ?? _errorHandler.DisplayLoadErrorMessage());
		}

		public async Task RemoveUser(int customerID, int userID, Action success, Action error = null) {
			await _clientProvider.Client.Put("/api/Customers/RemoveUser/" + customerID + "/" + userID, success, error ?? _errorHandler.DisplayLoadErrorMessage());
		}

        public async Task Delete(int customerID, Action success, Action error = null) {
            await _clientProvider.Client.Delete("/api/Customers/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Enable(int customerID, Action success, Action error = null) =>
            await _clientProvider.Client.Put("/api/Customers/Enable/" + customerID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());

        public async Task GetName(int id, Action<string> success, Action error = null) =>
            await _clientProvider.Client.Get("/api/Customers/GetCustomerName/" + id, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
    }
}
