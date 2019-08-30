using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibrary.Shared.Storage;
using RentApp;
using RentAppProject;

namespace Rent.Shared.ViewModels
{
    public class LoginVM
    {
        HttpClientProvider _clientProvider;
        LoginRepository _loginRepository;
        UserRepository _userRepository;
        UserVM _userVM;
        NotificationToken _token;

        public LoginVM(HttpClientProvider clientProvider, LoginRepository loginRepository, UserRepository userRepository, UserVM uservm, NotificationToken token)
        {
            _clientProvider = clientProvider;
            _loginRepository = loginRepository;
            _userRepository = userRepository;
            _userVM = uservm;
            _token = token;
        }

        public async void TryLogin(string username, string password, Action success, Action<string> error)
        {
            SaveLoad.SaveText("password", "");
            SaveLoad.SaveText("username", "");

            await _loginRepository.Login(new Login{ Password = password, UserName = username}, (obj) => {
				
                SaveLoad.SaveText("LoginToken", obj.Token);
                Login(obj);
                success();
            },() => {
                error("Indlæsningsfejl");
            });
        }

        public async void TryLoginWithToken(Action success, Action<string> error) 
        {
            /*
            if(SaveLoad.LoadText<string>("usertoken", out var token))
            {
                if (SaveLoad.LoadText<string>("username", out var username))
                {
                    TryLogin(username, password, success, error);
                } 
            }
            */


            if (SaveLoad.LoadText<string>("LoginToken", out var token))
            {
                _clientProvider.Client.DefaultRequestHeaders.Clear();
                _clientProvider.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                await _loginRepository.LoginWithToken(token, (obj) => {

                    SaveLoad.SaveText("LoginToken", obj.Token);
					Login(obj);
                    success();
                }, ErrorLoginWithToken);
            }
        }

        void ErrorLoginWithToken() {
            _clientProvider.Client.DefaultRequestHeaders.Clear();
        }

        async void Login(UserLoginInfo info)
        {
            _userVM.SetLoggedInUser(info.User);

            _clientProvider.Client.DefaultRequestHeaders.Clear();
            _clientProvider.Client.DefaultRequestHeaders.Add("Authorization", "Bearer " + info.Token);



            await _userRepository.RegisterForNotifications(_token, info.User.ID, () => { });
        }

        public async void LogOut()
        {
            await _loginRepository.Logout(() => { }, () => {
            });

            SaveLoad.SaveText("LoginToken", "");
            SaveLoad.SaveText("password", "");
            SaveLoad.SaveText("username", "");
        }
    }
}
