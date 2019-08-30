using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp
{
    public class UserRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;
        private readonly IUserVM _userVM;

        public UserRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler, IUserVM userVM)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
            _userVM = userVM;
        }

        public async Task RegisterForNotifications(NotificationToken token, int userID, Action success, Action error = null)
        {

            if (token.DeviceToken == null)
            {
                if(error != null) error.Invoke();
                return;
            }
            token.DeviceToken = token.DeviceToken.Replace("<", String.Empty);
            token.DeviceToken = token.DeviceToken.Replace(">", String.Empty);
            token.DeviceToken = token.DeviceToken.Replace(" ", String.Empty);

            await _clientProvider.Client.Post("/api/Notification/register/" + userID + "/apns/", token, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetUsers(Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetUsersWithRole(int roleID, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/WithRole/" + roleID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetLocationUsers(Location location, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetLocationUsers/" + location.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetCustomerUsers(Customer customer, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetCustomerUsers/" + customer.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetPotentialLocationUsers(Location location, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetPotentialLocationUsers/" + location.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Update(User user, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Users/", user, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task Delete(User user, Action success, Action error = null)
        {
            await _clientProvider.Client.Delete("/api/Users/" + user.ID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}