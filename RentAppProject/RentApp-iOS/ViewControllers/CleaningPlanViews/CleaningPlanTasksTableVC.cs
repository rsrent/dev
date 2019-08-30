using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using ModuleLibraryiOS.Alert;
using System.Linq;
using ModuleLibrary.Geo;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.ViewControllerInstanciater;
using static RentAppProject.CleaningTask;
using Rent.Shared.ViewModels;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class CleaningPlanTasksTableVC : ITableAndSourceViewController<CleaningTask>//ITableAndSourceGroupedViewController<CleaningTask>
    {
        CleaningPlanVM _cleaningPlanVM;
        CleaningTasksRepository _cleaningTasksRepository;
        CompletedCleaningTasksRepository _completedCleaningTasksRepository;
        UserVM _userVM;

        TableAndSourceController<CleaningPlanTasksTableVC, CleaningTask> tableController;

        public CleaningPlanTasksTableVC (IntPtr handle) : base (handle)
        {
            _cleaningPlanVM = AppDelegate.ServiceProvider.GetService<CleaningPlanVM>();
            _cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
            _completedCleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CompletedCleaningTasksRepository>();
            _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            tableController = TableAndSourceController<CleaningPlanTasksTableVC, CleaningTask>.Start(this);

            Title = _cleaningPlanVM.SchedulePlan.CleaningPlan.Description;
            TitleLabel.Text = _cleaningPlanVM.SchedulePlan.CleaningPlan.Description;

            if (_cleaningPlanVM.SchedulePlan.CleaningPlan.ID == 2)
                TitleLabel.Text = _cleaningPlanVM.SchedulePlan.CleaningPlan.Description;

            if(_cleaningPlanVM.ScheduleFloor != null) {
                TitleLabel.Text += ", " + _cleaningPlanVM.ScheduleFloor.Floor. Description.ToLower();
            }
			NavigationController.NavigationBar.Hidden = false;
            this.AddNavigationStack();
		}

		public override UITableViewCell GetCell(NSIndexPath path, CleaningTask val)
		{
            return TableFunctions.InstanciateCell<CleaningPlanTasksCell>(TableView, "CleaningPlanTasksCell", (cell) => { cell.UpdateCell(val); });
            /*
            if(val.Area.CleaningPlan.ID == 1)
                return TableFunctions.InstanciateCell<CleaningPlanTasksCustomerCell>(TableView, "CleaningPlanTasksCustomerCell", (cell) => { cell.UpdateCell(val); });
			else 
                return TableFunctions.InstanciateCell<CleaningPlanTasksCell>(TableView, "CleaningPlanTasksCell", (cell) => { cell.UpdateCell(val); });*/
		}

		public override UITableView GetTable() => TableView;

		public override async Task RequestTableData(Action<ICollection<CleaningTask>> updateAction)
		{
            await _cleaningTasksRepository.Get(_cleaningPlanVM.Location.ID, (schedule) =>
            {
                _cleaningPlanVM.CleaningSchedule.Schedule = schedule;
                _cleaningPlanVM.SchedulePlan = schedule.FirstOrDefault(p => p.CleaningPlan.ID == _cleaningPlanVM.SchedulePlan.CleaningPlan.ID);

                var newDic = new Dictionary<string, List<CleaningTask>>();
                if(_cleaningPlanVM.ScheduleFloor != null) {
                    
                    _cleaningPlanVM.ScheduleFloor = _cleaningPlanVM.SchedulePlan.Floors.FirstOrDefault(p => p.Floor.ID == _cleaningPlanVM.ScheduleFloor.Floor.ID);
                    foreach (var scheduleArea in _cleaningPlanVM.ScheduleFloor.Areas)
                    {
                        newDic.Add(scheduleArea.Area.Description, scheduleArea.Tasks.ToList());
                    }
                } else {
                    newDic.Add("", new List<CleaningTask>());
                    foreach (var scheduleArea in _cleaningPlanVM.SchedulePlan.Areas)
                    {
                        newDic[""].AddRange(scheduleArea.Tasks.ToList());
                        //newDic.Add(scheduleArea.Area.Description, scheduleArea.Tasks.ToList());
                    }
                }

                var list = new List<CleaningTask>();
                foreach(var kvp in newDic) {
                    list.AddRange(kvp.Value);
                }

                updateAction.Invoke(list);
            });
		}

		public override void RowSelected(NSIndexPath path, CleaningTask val)
		{
            var options = new List<(string, Action)>();

            if (_userVM.HasPermission(Permission.CompletedTask, Permission.CRUDD.Create))
            {
                options.Add(("Udfør opgave", () => {
                    Alert.DisplayTextField("Udfør opgave", "Tilføj en beskrivelse...", async (comment) => {

                        var coords = await Geo.GetPosition();
                        _completedCleaningTasksRepository.Create(
                            new CleaningTaskCompleted() { CompletedDate = DateTime.Now, Comment = comment, CleaningTaskID = val.ID }, () =>
                            {
                                Alert.DisplayToast("Completed task added", this);
                                tableController.ReloadTable();
                            }).LoadingOverlay(this);
                    }, this);
                }
                ));
            }

            if (_userVM.HasPermission(Permission.CompletedTask, Permission.CRUDD.Read))
            {
                options.Add(("Se historik", () =>
                {
                    _cleaningPlanVM.CleaningTask = val;
                    this.Start<CleaningPlanCompletedTasksTableVC>();
                }
                ));
            }

            if(_userVM.HasPermission(Permission.CleaningPlan, Permission.CRUDD.Delete)) {
                options.Add(("Slet opgave", () =>
                {
                    this.DisplayAlert("Slet opgave", "Du er ved at slette denne opgave, er du sikker på at du vil fortsætte?", new List<(string, Action)>()
                    {
                        ("Ja", async () => { await _cleaningTasksRepository.Delete(val, () => {
                            this.DisplayToast("Opgave slettet");
                        }); 
                        }),
                        ("Nej", () => {  })
                    });

                    _cleaningPlanVM.CleaningTask = val;
                    this.Start<CleaningPlanCompletedTasksTableVC>();
                }
                ));
            }

            if (options.Count > 0)
            {
                this.DisplayMenu(val.Area.Description + ", " + val.Comment, options, source: TableView.CellAt(path));
            }
		}
    }
}