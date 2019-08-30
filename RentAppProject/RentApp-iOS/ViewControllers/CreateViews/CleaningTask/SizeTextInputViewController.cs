using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace RentApp
{
    public partial class SizeTextInputViewController : UIViewController
    {
        private readonly CreateTaskVM _createTaskVM;

        public SizeTextInputViewController(IntPtr handle) : base(handle)
        {
            _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Button.TouchUpInside += (sender, e) => {

                if (Int16.TryParse(TextField.Text, out var sqm))
                {
                    _createTaskVM.Task.SquareMeters = sqm;
                }
            };
        }
    }
}