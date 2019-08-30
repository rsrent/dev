using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;
using Microsoft.Extensions.DependencyInjection;
using RentAppProject;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class UpdatePasswordVC : UIViewController
    {
        LoginRepository _loginRepository = AppDelegate.ServiceProvider.GetService<LoginRepository>();

        public UpdatePasswordVC (IntPtr handle) : base (handle)
        {
        }

        bool ForceUpdate;
        string Username;
        public void ParseInfo(bool forceUpdate, string username = null) {
            ForceUpdate = forceUpdate;
            Username = username;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            if (ForceUpdate)
            {
                UserName.Text = Username ?? "";

                OldPasswordStack.Hidden = true;
                this.RightNavigationButton("Opdater", () => {
                    _loginRepository.ForceUpdatePassword(new UpdateLogin
                    {
                        Login = new Login
                        {
                            UserName = UserName.Text,
                            Password = ""
                        },
                        NewPassword = NewPassword.Text
                    }, async () =>
                    {
                        this.DisplayToast("Kodeord opdateret");
                        await Task.Delay(200);
                        this.NavigationController.PopViewController(true);
                    }).LoadingOverlay(this);
                });
            }
            else
            {
                this.RightNavigationButton("Opdater", () => {
                    _loginRepository.UpdatePassword(new UpdateLogin
                    {
                        Login = new Login
                        {
                            UserName = UserName.Text,
                            Password = OldPassword.Text
                        },
                        NewPassword = NewPassword.Text
                    }, async () =>
                    {
                        this.DisplayToast("Kodeord opdateret");
                        await Task.Delay(200);
                        this.NavigationController.PopViewController(true);
                    }).LoadingOverlay(this);
                });

            }
        }
    }
}