using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace RentApp
{
    public partial class CleaningAssistentMenuVC : UIViewController
    {
        public CleaningAssistentMenuVC (IntPtr handle) : base (handle)
        {
        }
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            NavigationController.NavigationBar.Hidden = false;
            ViewLocationsButton.TouchUpInside += (sender, e) => {
                this.Start<LocationTableVC>().Set_ViewForUser();
            };
        }

        void ViewLocations() {
            //LocationTableVC.Start(this);
        }
    }
}