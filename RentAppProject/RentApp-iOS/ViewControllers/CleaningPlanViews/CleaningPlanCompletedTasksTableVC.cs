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
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class CleaningPlanCompletedTasksTableVC : ITableAndSourceViewController<CleaningTaskCompleted>
    {
        private readonly CompletedCleaningTasksRepository repository;
        private readonly CleaningPlanVM _cleaningPlanVM;

        public CleaningPlanCompletedTasksTableVC (IntPtr handle) : base (handle) 
        {
            repository = AppDelegate.ServiceProvider.GetService<CompletedCleaningTasksRepository>();
            _cleaningPlanVM = AppDelegate.ServiceProvider.GetService<CleaningPlanVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			TableAndSourceController<CleaningPlanCompletedTasksTableVC, CleaningTaskCompleted>.Start(this);
            //Title = _cleaningPlanVM.CleaningTask.Area.Description;
			NavigationController.NavigationBar.Hidden = false;

            TitleLabel.Text = "Udførte " + _cleaningPlanVM.CleaningTask.Area.Description + " opgaver";
            Title = TitleLabel.Text;
            this.AddNavigationStack();
		}

		public override UITableViewCell GetCell(NSIndexPath path, CleaningTaskCompleted val)
        {
			return TableView.StartCell<CleaningPlanCompletedTasksCell>((cell) => cell.UpdateCell(val));
		}

        public override UITableView GetTable() => TableView;

        public override async Task RequestTableData(Action<ICollection<CleaningTaskCompleted>> updateAction)
        {
            await repository.Get(_cleaningPlanVM.CleaningTask, (list) => updateAction.Invoke(list.OrderByDescending(t => t.CompletedDate).ToList()));
		}
    }
}