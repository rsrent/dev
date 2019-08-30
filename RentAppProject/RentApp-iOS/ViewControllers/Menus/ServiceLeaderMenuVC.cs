using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class ServiceLeaderMenuVC : UIViewController
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();

        public ServiceLeaderMenuVC (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			NavigationController.NavigationBar.Hidden = false;
            Title = _userVM.LoggedInUser().FirstName + " " + _userVM.LoggedInUser().LastName;

			ViewCustomersButton.TouchUpInside += (sender, e) => {
				this.Start<LocationTableVC>().Set_ViewForUser();
			};
        }
    }
}