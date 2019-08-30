using Foundation;
using System;
using UIKit;

namespace ModuleLibraryiOS.Login
{
    public partial class LoginViewController : UIViewController
    {
		public LoginViewController(IntPtr handle) : base(handle)
		{
		}

        private Action<string> attemptLogin;

        public static void Start(UIViewController viewController, Action<string> login)
		{
			var chatStoryboard = UIStoryboard.FromName("Login", null);
			var newView = chatStoryboard.InstantiateViewController("LoginViewController") as LoginViewController;
			if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
			newView.parseInfo(login);
		}

        private void parseInfo(Action<string> login) {
            attemptLogin = login;
        }

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			if (NavigationController != null) NavigationController.NavigationBar.Hidden = true;
		}

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SignInButton.TouchUpInside += (sender, e) => {
                attemptLogin.Invoke(UserNameTextField.Text);
            };
        }
    }
}