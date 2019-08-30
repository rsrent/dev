using Foundation;
using System;
using UIKit;

namespace ModuleLibraryiOS.Profile
{
    public partial class ProfileViewController : UIViewController
    {
        public ProfileViewController (IntPtr handle) : base (handle)
        {
        }


		public static void Start(UIView container, UIViewController viewController)
		{
			var chatStoryboard = UIStoryboard.FromName("Profile", null);
			var newView = chatStoryboard.InstantiateViewController("ProfileViewController") as ProfileViewController;
			newView.parseInfo();
			if (container != null && viewController != null)
			{
				newView.View.Frame = container.Bounds;
				newView.WillMoveToParentViewController(viewController);

				container.AddSubview(newView.View);
				viewController.AddChildViewController(newView);
				newView.DidMoveToParentViewController(viewController);
			}
			else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
		}

        private void parseInfo() {
            
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }
    }
}