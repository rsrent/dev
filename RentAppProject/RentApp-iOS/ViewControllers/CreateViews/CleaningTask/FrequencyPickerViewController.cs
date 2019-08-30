using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Input;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.ViewControllerInstanciater;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class FrequencyPickerViewController : IPickerViewController
    {
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
        private readonly CleaningTasksRepository cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();

        public FrequencyPickerViewController (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
            PickerController<FrequencyPickerViewController>.Start(this);

			Button.TouchUpInside += (sender, e) => {
				var timesYearly = new int[] { 52, 26, 12, 6, 4, 3, 2, 1 };
				_createTaskVM.Task.TimesOfYear = (byte)timesYearly[Picker.SelectedRowInComponent(0)];
                _createTaskVM.Task.Floor = _createTaskVM.Floor;
                _createTaskVM.Task.Area = _createTaskVM.Area;
                _createTaskVM.Task.Frequency = null;
                if(_createTaskVM.Task.Area.CleaningPlanID == 1) 
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
			};

            this.RightNavigationButton("Anden",(obj) => {
                this.Start<OtherFrequencyViewController>();
            });
		}

        public override UIPickerView GetPicker()
        {
            return Picker;
        }

        public override string[][] GetPickerOptions()
        {
            var timesYearly = new string[] { "Hver uge (52 gange)", "Hver anden uge (26 gange)", "Hver måned (12 gange)", "Hver anden måned (6 gange)", "Hver tredje måned (4 gange)", "Hver fjerde måned (3 gange)", "Hver 6 måned (2 gange)", "En gang årligt (1 gang)" };
            return new string[][] { timesYearly };
        }


    }
}