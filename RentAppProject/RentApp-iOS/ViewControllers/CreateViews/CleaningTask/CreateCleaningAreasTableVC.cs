using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using RentAppProject;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using RentApp.Shared.Repositories;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;

namespace RentApp
{
    public partial class CreateCleaningAreasTableVC : ITableAndSourceViewController<Area>
    {
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();

        public CreateCleaningAreasTableVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var TableController = TableAndSourceController<CreateCleaningAreasTableVC, Area>.Start(this);
            _createTaskVM.SameAreaViewController = this;

            this.RightNavigationButton("Tilføj område", () =>
            {
                Alert.DisplayTextField("Navngiv nyt område", "e.g. toilet", (custom) =>
                {
                    _floorAreaRepository.AddArea(_createTaskVM.CleaningPlan.ID, new Area { Description = custom }, (obj) =>
                    {
                        TableController.ReloadTable();
                    }).LoadingOverlay(this, "Tilføjer område");
                }, this);
            });

            Title = "";
            if (_createTaskVM.CleaningPlan.HasFloors)
                TitleLabel.Text = "Vælg område til " + _createTaskVM.Floor.Description;
            else 
                TitleLabel.Text = "Vælg opgave til " + _createTaskVM.CleaningPlan.Description;
        }

        public override UITableViewCell GetCell(NSIndexPath path, Area val)
        {
            return Table.StartCell<CreateCleaningAreaCell>(c => c.TextLabel.Text = val.Description);
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<ICollection<Area>> updateAction)
        {
            await _floorAreaRepository.Areas(_createTaskVM.CleaningPlan.ID, (areas) =>
            {
                updateAction(areas);
            });
        }

        public override void RowSelected(NSIndexPath path, Area val)
        {
            _createTaskVM.Area = val;
            if(_createTaskVM.CleaningPlan.HasFloors) {
                this.Start<SizeTextInputViewController>();

            } else {
                this.Start<FrequencyPickerViewController>();
            }
        }
    }
}