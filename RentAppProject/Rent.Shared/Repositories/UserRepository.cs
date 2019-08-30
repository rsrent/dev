﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;
//using RentAppProject;

namespace RentApp
{
    public class UserRepository
    {
        private readonly HttpClientProvider _clientProvider;
        //private readonly IUserVM _userVM;

        public UserRepository(HttpClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
            //_userVM = userVM;
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

            await _clientProvider.Client.Post("/api/Notification/register/" + userID + "/apns/", token, successA: success, errorA: error);
        }

        public async Task GetUsers(Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/", successA: success, errorA: error);
        }

        public async Task GetUsersWithRole(int roleID, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/WithRole/" + roleID, successA: success, errorA: error);
        }

        public async Task GetLocationUsers(Location location, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetLocationUsers/" + location.ID, successA: success, errorA: error);
        }

        public async Task GetCustomerUsers(Customer customer, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetCustomerUsers/" + customer.ID, successA: success, errorA: error);
        }

        public async Task GetPotentialLocationUsers(Location location, Action<ICollection<User>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Users/GetPotentialLocationUsers/" + location.ID, successA: success, errorA: error);
        }

        public async Task Update(User user, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Users/", user, successA: success, errorA: error);
        }

        public async Task Disable(User user, Action success, Action error = null)
        {
            await _clientProvider.Client.Delete("/api/Users/" + user.ID, successA: success, errorA: error);
        }

        public async Task Enable(User user, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Users/Enable/" + user.ID, successA: success, errorA: error);
        }
    }
}