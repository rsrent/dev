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
        private readonly IErrorMessageHandler _errorHandler;
        private readonly IUserVM _userVM;

        public LoginRepository(HttpClientProvider clientProvider, IErrorMessageHandler errorHandler, IUserVM userVM)
        {
            _clientProvider = clientProvider;
            _errorHandler = errorHandler;
            _userVM = userVM;
        }

        public async Task Login(Login login, Action<UserLoginInfo> success, Action error = null)
        {
            Action<UserLoginInfo> ExtraSuccess = (info) =>
            {
                if (info.User != null) _userVM.SetLoggedInUser(info.User);
                else
                {
                    if(error != null)
                        error.Invoke();
                    return;
                }
                _clientProvider.Client.DefaultRequestHeaders.Clear();
                _clientProvider.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + info.Token);

                success.Invoke(info);
            };

            await _clientProvider.Client.Post("/api/Logins/Login/", login, ExtraSuccess, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task LoginWithToken(string token, Action<UserLoginInfo> success, Action error = null)
        {
            _clientProvider.Client.DefaultRequestHeaders.Clear();
            _clientProvider.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

            Action<UserLoginInfo> ExtraSuccess = (info) =>
            {
                if (info.User != null) _userVM.SetLoggedInUser(info.User);
                else
                {
                    if (error != null)
                        error.Invoke();
                    return;
                }
                success.Invoke(info);
            };

            Action ExtraFailure = () =>
            {
                _clientProvider.Client.DefaultRequestHeaders.Clear();
                if (error != null)
                    error.Invoke();
            };

            await _clientProvider.Client.Post("/api/Logins/LoginWithToken/", successA: ExtraSuccess, errorA: ExtraFailure);
        }

        public async Task CreateUser(UserLoginInfo info, Action<User> success, Action error = null)
        {
            await _clientProvider.Client.Post("/api/Logins/CreateLogin/", info, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task UpdatePassword(UpdateLogin updatedLogin, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Logins/UpdatePassword/", updatedLogin, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }

        public async Task ForceUpdatePassword(UpdateLogin updatedLogin, Action success, Action error = null)
        {
            await _clientProvider.Client.Put("/api/Logins/ForceUpdatePassword/", updatedLogin, success, error ?? _errorHandler.DisplayLoadErrorMessage());
        }
    }
}