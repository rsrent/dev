using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Navigation;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class CleaningPlansTableVC : ITableAndSourceViewController<CleaningSchedule.SchedulePlan>
    {
        CleaningTasksRepository _cleaningTasksRepository;
        UserVM _userVM;
        CleaningPlanVM _cleaningPlanVM;
        TableAndSourceController<CleaningPlansTableVC, CleaningSchedule.SchedulePlan> TableController;

        public CleaningPlansTableVC (IntPtr handle) : base (handle)
        {
            _cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
            _cleaningPlanVM = AppDelegate.ServiceProvider.GetService<CleaningPlanVM>();
            _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            _cleaningPlanVM.CleaningSchedule = new CleaningSchedule();
            TableController = TableAndSourceController<CleaningPlansTableVC, CleaningSchedule.SchedulePlan>.Start(this);
			NavigationController.NavigationBar.Hidden = false;

            Title = "Planer";
            TitleLabel.Text = "Rengøringsplaner";
            this.AddNavigationStack();
		}

        public override UITableViewCell GetCell(NSIndexPath path, CleaningSchedule.SchedulePlan val)
		{
            return TableView.StartCell<CleaningPlansCell>((cell) => { cell.TextLabel.Text = val.CleaningPlan.Description; });
		}

		public override UITableView GetTable() => TableView;

        public override async Task RequestTableData(Action<ICollection<CleaningSchedule.SchedulePlan>> updateAction)
		{
            await _cleaningTasksRepository.Get(_cleaningPlanVM.Location.ID, (schedule) =>
            {
                _cleaningPlanVM.CleaningSchedule.Schedule = schedule;
                updateAction.Invoke(_cleaningPlanVM.CleaningSchedule.Schedule);
            });
		}

        public override void RowSelected(NSIndexPath path, CleaningSchedule.SchedulePlan val)
		{
            _cleaningPlanVM.SchedulePlan = val;

            if(val.Floors.Count > 0) {
                Starter.Start<CleaningPlanFloorsTableVC>(this);

            } else {
                _cleaningPlanVM.ScheduleFloor = null;
                Starter.Start<CleaningPlanTasksTableVC>(this);
            }
		}

        /*
        Func<Task> ReloadTable;
        public override void ParseReloadFunction(Func<Task> Reload)
        {
            ReloadTable = Reload;
        } */

        bool hasAppeared;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!hasAppeared)
                hasAppeared = true;
            else
                TableController.ReloadTable();

            if (_userVM.HasPermission(Permission.CleaningPlan, Permission.CRUDD.Create))
            {
                this.RightNavigationButton("Ny opgave", () => {
                    AppDelegate.ServiceProvider.GetService<CreateTaskVM>().Location = _cleaningPlanVM.Location;
                    AppDelegate.ServiceProvider.GetService<CreateTaskVM>().QualityReport = null;
                    //this.Start<CreateCleaningPlansTableVC>();



                    this.Start<TaskCreateVC>().NewTask(_cleaningPlanVM.Location.ID);
                });
            }
        }
    }
}