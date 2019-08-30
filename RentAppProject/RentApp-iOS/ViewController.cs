using System;

using UIKit;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;

namespace RentApp
{
    public partial class ViewController : UIViewController
    {
        protected ViewController(IntPtr handle) : base(handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var errorMessageHandler = AppDelegate.ServiceProvider.GetService<IErrorMessageHandler>() as ErrorMessageHandler;
            errorMessageHandler.SetNavigation(NavigationController);
            NavigationController.InteractivePopGestureRecognizer.Enabled = false;
            AppDelegate.ServiceProvider.GetService<RootViewModel>().Root = this;
            LoginVC.Start(this);
		}

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            NavigationController.NavigationBar.Hidden = true;
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
        }
    }
}
