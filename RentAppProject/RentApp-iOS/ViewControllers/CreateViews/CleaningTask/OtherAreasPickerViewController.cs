using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Input;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace RentApp
{
    public partial class OtherAreasPickerViewController : IPickerViewController
    {
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();

        public OtherAreasPickerViewController (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
            PickerController<OtherAreasPickerViewController>.Start(this);

			Button.TouchUpInside += async (sender, e) => {
				string area = GetPickerOptions()[0][Picker.SelectedRowInComponent(0)];

				TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
				tcs.SetResult(true);
				if (area.Equals("Custom"))
				{
					tcs = new TaskCompletionSource<bool>();
					Alert.DisplayTextField("Indtast navn på opgaven", "Opgaven", (custom) =>
					{
						area = custom;
						tcs.SetResult(true);
					}, this);
				}
				await tcs.Task;

                _createTaskVM.Task.Area = new Area { Description = area };
                _createTaskVM.Task.SquareMeters = 0;
			};
		}

		public override UIPickerView GetPicker()
        {
            return Picker;
        }

        public override string[][] GetPickerOptions()
        {
            if(_createTaskVM.Task.Area.CleaningPlanID == 2) {
                return new[] { new [] {
                    "Åbne udstillingsvinduer indvendigt",
                    "Lukkede udstillingsvinduer indvendigt",
                    "Facade/døre/udstillingsvinduer i gaden",
                    "Facade/udstillingsvinduer 1. sal udvendigt",
                    "Indgangspartier indvendigt",
                    "Vindusflader i butik over 1.80m + udvendig gelænderglas, repoglas og rulletrappe",
                    "Gelænder, repoglas, rulletrappe indvendig",
                    "Elevator glasflader i nå højde + døre på 2 sider indvendig",
                    "Baglokale + internt glas udvendig og indvendig",
                        "Custom"
                    }
                };
            } else {
                return new [] {
                    new string[]{ "Fan coil"}
                };
            }
        }
    }
}