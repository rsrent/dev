using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;
using static RentAppProject.Permission;

namespace RentApp
{
    public class PermissionRepository
    {
        private readonly HttpClientProvider _clientProvider;
        private readonly IErrorMessageHandler _errorHandler;
        private readonly IUserVM _userVM;

        public PermissionRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler, IUserVM userVM)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
            _userVM = userVM;
        }

        public async Task GetPermissions(Action<ICollection<Permission>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Permission/", successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetUserPermissions(int userID, Action<ICollection<UserPermission>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Permission/GetUserPermissions/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task UpdateUserPermission(UserPermission up, CRUDD cruud, bool active, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Permission/UpdateUserPermission/" + up.UserID + "/" + up.PermissionID + "/" + cruud + "/" + active, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task CreatePermission(Permission permission, Action success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Permission/", content: permission, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task GetPermissionTemplates(int roleID, Action<List<PermissionTemplate>> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Permission/GetPermissionTemplates/" + roleID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task UpdatePermissionTemplate(PermissionTemplate pt, CRUDD cruud, bool active, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Permission/UpdatePermissionTemplate/" + pt.RoleID + "/" + pt.PermissionID + "/" + cruud + "/" + active, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task ResetUserPermissions(int userID, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Permission/ResetUserPermissions/" + userID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
        public async Task ResetUsersSpecificPermissions(int permissionID, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Permission/ResetUsersSpecificPermissions/" + permissionID, successA: success, errorA: error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}
