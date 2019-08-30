using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using RentAppProject;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentApp.Shared.Repositories;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;

namespace RentApp
{
    public partial class CreateCleaningPlansTableVC : ITableAndSourceViewController<CleaningPlan>
    {
        private readonly FloorAreaRepository _floorAreaRepository = AppDelegate.ServiceProvider.GetService<FloorAreaRepository>();
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();

        public CreateCleaningPlansTableVC (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceController<CreateCleaningPlansTableVC, CleaningPlan>.Start(this);
            _createTaskVM.RootViewController = this;
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            _createTaskVM.Task = new CleaningTask();
            _createTaskVM.CleaningPlan = null;
            _createTaskVM.Floor = null;
            _createTaskVM.Area = null;

            Title = "";
            TitleLabel.Text = "Vælg plan";
        }

        public override UITableViewCell GetCell(NSIndexPath path, CleaningPlan val)
        {
            return Table.StartCell<CreateCleaningPlanCell>(c => c.TextLabel.Text = val.Description);
        }

        public override UITableView GetTable() => Table;

        public override async Task RequestTableData(Action<ICollection<CleaningPlan>> updateAction)
        {
            await _floorAreaRepository.Plans((plans) => {
                updateAction(plans);
            });
        }

        public override void RowSelected(NSIndexPath path, CleaningPlan val)
        {
            _createTaskVM.CleaningPlan = val;
            if(val.HasFloors) {
                this.Start<CreateCleaningFloorsTableVC>();
            } else {
                this.Start<CreateCleaningAreasTableVC>();
            }
        }
    }
}