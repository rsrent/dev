using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using System.Linq;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class CleaningPlanFloorsTableVC : ITableAndSourceViewController<CleaningSchedule.ScheduleFloor>
    {
        CleaningTasksRepository repository;
        CleaningPlanVM _cleaningPlanVM;
        TableAndSourceController<CleaningPlanFloorsTableVC, CleaningSchedule.ScheduleFloor> TableController;

		public CleaningPlanFloorsTableVC (IntPtr handle) : base (handle)
        {
            repository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
            _cleaningPlanVM = AppDelegate.ServiceProvider.GetService<CleaningPlanVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableController = TableAndSourceController<CleaningPlanFloorsTableVC, CleaningSchedule.ScheduleFloor>.Start(this);
            //Title = _cleaningPlanVM.SchedulePlan.CleaningPlan.Description;
			NavigationController.NavigationBar.Hidden = false;

            TitleLabel.Text = "Etager";

            Title = "Etager";
            this.AddNavigationStack();
		}

        public override UITableViewCell GetCell(NSIndexPath path, CleaningSchedule.ScheduleFloor val)
		{
            return TableFunctions.InstanciateCell<CleaningPlanFloorsCell>(TableView, "CleaningPlanFloorsCell", (cell) => { cell.TextLabel.Text = val.Floor.Description; });
		}

		public override UITableView GetTable() => TableView;

        public override async Task RequestTableData(Action<ICollection<CleaningSchedule.ScheduleFloor>> updateAction)
		{
            await repository.Get(_cleaningPlanVM.Location.ID, (schedule) => {
                _cleaningPlanVM.CleaningSchedule.Schedule = schedule;
                _cleaningPlanVM.SchedulePlan = schedule.FirstOrDefault(p => p.CleaningPlan.ID == _cleaningPlanVM.SchedulePlan.CleaningPlan.ID);
                updateAction.Invoke(_cleaningPlanVM.SchedulePlan.Floors);
            });
		}

        public override void RowSelected(NSIndexPath path, CleaningSchedule.ScheduleFloor val)
		{
            _cleaningPlanVM.ScheduleFloor = val;
            this.Start<CleaningPlanTasksTableVC>();
		}

        bool hasAppeared;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!hasAppeared)
                hasAppeared = true;
            else
                TableController.ReloadTable();
        }
    }
}