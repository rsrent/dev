using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp
{
    public class RoleRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;

		public RoleRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler)
		{
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
		}

        public async Task GetRoles(Action<List<Role>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Role", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task AddRole(Role role, Action success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Role/AddRole", content: role, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task UpdateUserRole(int userID, int roleID, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Role/UpdateUserRole/" + userID + "/" + roleID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

    }
}
