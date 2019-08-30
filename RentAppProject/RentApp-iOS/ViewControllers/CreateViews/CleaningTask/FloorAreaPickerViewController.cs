using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Input;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using System.Collections.Generic;
using System.Linq;
using RentApp.Shared.Repositories;

namespace RentApp
{
    /*
    public partial class FloorAreaPickerViewController : IPickerViewController
    {
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();

        public FloorAreaPickerViewController(IntPtr handle) : base(handle) { }

        List<Floor> Floors;
        List<Area> Areas;

		public override void ViewDidLoad()
		{
            _createTaskVM.RootViewController = this;

			Button.TouchUpInside += async (sender, e) => {

                bool CustomFloor = Picker.SelectedRowInComponent(0) == Floors.Count;
                bool CustomArea = Picker.SelectedRowInComponent(1) == Areas.Count;

                Floor floor = null;
                Area area = null;

				TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
				tcs.SetResult(true);

                if (CustomFloor)
				{
					tcs = new TaskCompletionSource<bool>();
					Alert.DisplayTextField("Navngiv special etage", "e.g. 45.", async (custom) =>
					{
                        await _floorAreaRepository.AddFloor(new Floor
                        {
                            Description = custom
                        }, (obj) => {
                            floor = obj;
                            tcs.SetResult(true);
                        });
					}, this);
                    await tcs.Task;
                } else {
                    floor = Floors[(int)Picker.SelectedRowInComponent(0)];
                }

                if (CustomArea)
				{
					tcs = new TaskCompletionSource<bool>();
					Alert.DisplayTextField("Navngiv special område", "e.g. toilet", async (custom) =>
					{
                        await _floorAreaRepository.AddArea(1, new Area
                        {
                            Description = custom
                        }, (obj) =>
                        {
                            area = obj;
                            tcs.SetResult(true);
                        });
					}, this);
                    await tcs.Task;
                } else {
                    area = Areas[(int)Picker.SelectedRowInComponent(1)];
                }

                //_createTaskVM.Task.CleaningPlan = new CleaningPlan { ID = 1, Description = "Regular"};
                _createTaskVM.Task.Floor = floor;
                _createTaskVM.Task.Area = area;


                //_createTaskVM.Task = new CleaningTask { Area = new Area { Description = area }, Floor = new Floor { Description = floor }, PlanType = _createTaskVM.Type };
				//SizeTextInputViewController.Start(this).ParseInfo(Location, task, this, Report);
			};

            this.RightNavigationButton("Andre", () => this.Start<OtherAreasTableViewController>());

            GetFloorsAndAreas();
		}

        async void GetFloorsAndAreas() {

            this.DisplayLoadingWhile(() => _floorAreaRepository.Areas(async (areas) => {
                await _floorAreaRepository.Floors((floors) => {
                    Floors = floors;
                    Areas = areas;
                    PickerController<FloorAreaPickerViewController>.Start(this);
                });
            }));
        }

		public override UIPickerView GetPicker() { return Picker; }

        public override string[][] GetPickerOptions()
        {
            return new[] {
                Floors.Select(f => f.Description).ToArray().Append("Custom").ToArray(), Areas.Select(a => a.Description).ToArray().Append("Custom").ToArray()


                //new[] { "Kælder", "Stuen", "1.", "2.", "3.", "4.", "5.", "6.", "7.", "8.", "9.", "10.", "Custom" },

                //new[] { "Indgang", "Gang", "Reception", "Trapper", "Kontor", "Mødelokale", "Køkken/Kantine", "Udstilling", "Showroom", "Butiksgulv", "Toilet", "Baderum/omklædning", "Lager", "Værksted", "Kaffemaskine", "Køleskab", "Custom" } 
            };
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            _createTaskVM.Task = null;
        }
    }
    */
}