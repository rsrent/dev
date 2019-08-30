using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentApp.Shared.Models;
using RentAppProject;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Alert;
using System.Linq;
using System.Collections.Generic;
using ModuleLibraryiOS.Services;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class LoginVC : UIViewController
    {
        public LoginVC (IntPtr handle) : base (handle) { }
		public static LoginVC Start(UIViewController vc)
		{
			return Starter.Start<LoginVC>(vc, "Main", "LoginVC");
		}

        UserVM userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        LoginVM loginVM = AppDelegate.ServiceProvider.GetService<LoginVM>();
        CustomerVM customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        LocationVM locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();

        UserRepository userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        LoginRepository _loginRepository = AppDelegate.ServiceProvider.GetService<LoginRepository>();
        CustomerRepository customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        LocationRepository locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();


        bool TryingToLogin;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.Hidden = true;
            LoginButton.TouchUpInside += (sender, e) => {
                //if(!TryingToLogin) TryLogin();
                loginVM.TryLogin(UsernameLabel.Text, PasswordLabel.Text, Success, Error);
            };

            LiveButton.TouchUpInside += (sender, e) => {
                UpdateProductionState(!AppDelegate.ServiceProvider.GetService<Settings>().Live);
            };
            //LiveButton.Hidden = true;

            UpdateProductionState(AppDelegate.ServiceProvider.GetService<Settings>().Live);

            TryLoginWithToken();
        }

        async void TryLoginWithToken() {
            await Task.Delay(100);

            loginVM.TryLoginWithToken(Success, Error);
        }
        /*
        async void TryLoginWithToken() {
            await Task.Delay(100);
            if (SaveLoad.LoadText<string>("LoginToken", out var token))
            {
                _loginRepository.LoginWithToken(token, Success).LoadingOverlay(this, "");
                //this.DisplayLoadingWhile(() => _loginRepository.LoginWithToken(token, Success), "");
            }
        }*/

        void UpdateProductionState(bool live) {
            LiveButton.SetTitle(live ? "Is Live" : "Is Development", UIControlState.Normal);
            AppDelegate.SetLive(live);

            if (AppDelegate.ServiceProvider.GetService<Settings>().Live)
            {
                View.BackgroundColor = UIColor.White;
            } else {
                View.BackgroundColor = UIColor.FromRGB(70, 230, 255);
            }
        }
        /*
        void TryLogin() {
            TryingToLogin = true;

            this.DisplayLoadingWhile(async () => {
                var userName = UsernameLabel.Text;
                var password = PasswordLabel.Text;

                var login = new Login
                {
                    UserName = userName,
                    Password = password
                };

                await _loginRepository.Login(login, Success);

                TryingToLogin = false;
            });
        } */

        void Success() 
        {
            //userVM.SetLoggedInUser(info.User);

            //SaveLoad.SaveText("LoginToken", info.Token);

            base.NavigationController.PopViewController(true);
            this.Start<TabBarController>();

            //await RegisterForNotifications(AppDelegate.ServiceProvider.GetService<NotificationToken>(), userVM.LoggedInUser().ID);
        }

        public void Error(string message) 
        {
            this.DisplayToast("Kunne ikke logge ind");
        }
        /*
		public async Task RegisterForNotifications(NotificationToken token, int userID)
		{
			await userRepository.RegisterForNotifications(token, userID, () => { });
		} 

        void SetRootLeftButton(UIViewController rootVC) {
            rootVC.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Profile", UIBarButtonItemStyle.Plain, (sender, e) =>
            {
                var user = userVM.LoggedInUser();
                rootVC.DisplayMenu(user.FirstName + " " + user.LastName, new List<(string, Action)> {
                    ("View profile", () => { rootVC.NavigationController.TopViewController.Start<EmployeeProfile>().ParseInfo(user); }),
                    ("Log out", async () => { 
                        rootVC.NavigationController.PopToRootViewController(true); 
                        await Task.Delay(200);
                        LoginVC.Start(AppDelegate.ServiceProvider.GetService<RootViewModel>().Root); 
                    })
                });

            });
        }
        */
    }
}