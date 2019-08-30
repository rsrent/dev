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
    public partial class CreateCleaningFloorsTableVC : ITableAndSourceViewController<Floor>
    {
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();

        public CreateCleaningFloorsTableVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            var TableController = TableAndSourceController<CreateCleaningFloorsTableVC, Floor>.Start(this);

            this.RightNavigationButton("Tilføj etage", () =>
            {
                this.DisplayTextField("Navngiv ny etage", "etage...", (custom) =>
                {
                    _floorAreaRepository.AddFloor(new Floor { Description = custom }, (obj) =>
                    {
                        TableController.ReloadTable();
                    }).LoadingOverlay(this, "Tilføjer etage");
                });
            });

            Title = "";
            TitleLabel.Text = "Vælg etage til " + _createTaskVM.CleaningPlan.Description.ToLower();
        }

        public override UITableViewCell GetCell(NSIndexPath path, Floor val)
        {
            return Table.StartCell<CreateCleaningFloorCell>(c => c.TextLabel.Text = val.Description);
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<ICollection<Floor>> updateAction)
        {
            await _floorAreaRepository.Floors((floors) =>
            {
                updateAction(floors);
            });
        }

        public override void RowSelected(NSIndexPath path, Floor val)
        {
            _createTaskVM.Floor = val;
            this.Start<CreateCleaningAreasTableVC>();
        }
    }
}