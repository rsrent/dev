using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Alert;
using RentApp.ViewModels;

namespace RentApp
{
    public partial class OtherFrequencyViewController : UIViewController
    {
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
        private readonly CleaningTasksRepository cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();

        public OtherFrequencyViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();


            Button.TouchUpInside += (sender, e) => {

                if(byte.TryParse(TextField.Text, out var frequency)) 
                {
                    _createTaskVM.Task.TimesOfYear = frequency;
                    _createTaskVM.Task.Floor = _createTaskVM.Floor;
                    _createTaskVM.Task.Area = _createTaskVM.Area;
                    _createTaskVM.Task.Frequency = null;
                    if (_createTaskVM.Task.Area.CleaningPlanID == 1)
                    {
                        this.Start<NameAreaViewController>();
                    }
                    else
                    {
                        cleaningTasksRepository.Add(_createTaskVM.Location.ID, _createTaskVM.Task, (task) =>
                        {
                            NavigationController.PopToViewController(_createTaskVM.RootViewController, true);
                            Alert.DisplayToast("Opgave tilføjet", _createTaskVM.RootViewController);
                        }, () => Alert.DisplayToast("Indlæsningsfejl", this)).LoadingOverlay(this);
                    }
                }
                else Alert.DisplayToast("Frekvens er ikke et tal", this);


            };
        }
    }
}