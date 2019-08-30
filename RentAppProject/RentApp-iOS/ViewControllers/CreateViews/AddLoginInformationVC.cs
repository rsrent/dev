using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class AddLoginInformationVC : UIViewController
    {
		//CreateUserVM _createUserVM;
        LoginRepository _loginRepository;
        UserLoginInfo UserLoginInfo;
        Func<User, Task> SuccesssAction;
        public AddLoginInformationVC (IntPtr handle) : base (handle)
        {
            //_createUserVM = AppDelegate.ServiceProvider.GetService<CreateUserVM>();
            _loginRepository = AppDelegate.ServiceProvider.GetService<LoginRepository>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            this.RightNavigationButton("Tilføj", () => {

                if(PasswordTF.Text.Length < 6)
                {
                    this.DisplayToast("Kodeord skal være mindst 6 karakterer langt");
                }

                UserLoginInfo.Login = new Login { UserName = UserNameTF.Text, Password = PasswordTF.Text };

                _loginRepository.CreateUser(UserLoginInfo, Success).LoadingOverlay(this);
            });
        }

        void Success(User user) {
            SuccesssAction(user).LoadingOverlay(this);
        } 

        public void SetUser(UserLoginInfo info, Func<User, Task> success) {
            UserLoginInfo = info;
            SuccesssAction = success;
        }
    }
}