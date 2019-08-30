using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class FrequencyViewController : UIViewController
    {
        private readonly CreateTaskVM _createTaskVM;

        public FrequencyViewController (IntPtr handle) : base (handle)
        {
            _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Button.TouchUpInside += (sender, e) => {

                if (Int16.TryParse(TextField.Text, out var frequenzy))
				{
					_createTaskVM.Task.Frequency = frequenzy + "";
                    _createTaskVM.Task.TimesOfYear = null;
                    //NameAreaViewController.Start(this).ParseInfo(Location, Task, Root, Report);
				}
			};

            this.RightNavigationButton("Som interval", () => { this.Start<FrequencyPickerViewController>(); });

            /*
			NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Som interval", UIBarButtonItemStyle.Plain, (sender, e) => {
                FrequencyPickerViewController.Start(this).ParseInfo(Location, Task, Root, Report);
			}), true); */
		}
    }
}