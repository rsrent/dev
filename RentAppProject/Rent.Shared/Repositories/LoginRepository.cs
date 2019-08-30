using System;
using System.Net.Http;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;

namespace RentApp
{
    public class LoginRepository
    {
        private readonly HttpClientProvider _clientProvider;

        public LoginRepository(HttpClientProvider clientProvider)
        {
            _clientProvider = clientProvider;
        }

        public async Task Login(Login login, Action<UserLoginInfo> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Logins/Login/", login, successA: success, errorA: error);
        }

        public async Task LoginWithToken(string token, Action<UserLoginInfo> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Logins/LoginWithToken/", successA: success, errorA: error);
        }

        public async Task CreateUser(UserLoginInfo info, Action<User> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Logins/CreateLogin/", info, success, error);
        }

        public async Task UpdatePassword(UpdateLogin updatedLogin, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Logins/UpdatePassword/", updatedLogin, success, error);
        }

        public async Task ForceUpdatePassword(UpdateLogin updatedLogin, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Logins/ForceUpdatePassword/", updatedLogin, success, error);
        }

        public async Task Logout(Action success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Logins/Logout", successA: success, errorA: error);
        }

        public async Task GetUserUsername(int userId, Action<string> success, Action error = null)
        {
            await _clientProvider.Client.Get("/api/Logins/GetUserUsername/" + userId, successA: success, errorA: error);
        }
    }
}