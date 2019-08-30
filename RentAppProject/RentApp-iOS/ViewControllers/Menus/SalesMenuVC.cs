using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace RentApp
{
    public partial class SalesMenuVC : UIViewController
    {
        public SalesMenuVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.Hidden = false;
        }
    }
}