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
using RentApp.Shared.Repositories;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class QualityReportFloorsTableVC : ITableAndSourceViewController<string>
    {
		QualityReportRepository qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
        UserVM userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        QualityReportVM _qualityReportVM = AppDelegate.ServiceProvider.GetService<QualityReportVM>();
        LocationVM locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();
        TableAndSourceController<QualityReportFloorsTableVC, string> TableController;

        public QualityReportFloorsTableVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableController = TableAndSourceController<QualityReportFloorsTableVC, string>.Start(this);
            TitleLabel.Text = "Etager";
			NavigationController.NavigationBar.Hidden = false;
		}
		
		public override UITableViewCell GetCell(NSIndexPath path, string val)
		{
            return TableView.StartCell<QualityReportFloorsCell>((cell) => { cell.TextLabel.Text = val; });
		}

		public override UITableView GetTable()
        {
            return TableView;
        }

		public override async Task RequestTableData(Action<ICollection<string>> updateAction)
		{
            await qualityReportRepository.Get(_qualityReportVM.QualityReport.ID, (report) =>
            {
                _qualityReportVM.QualityReport = report;
                updateAction.Invoke(report.Floors.Select(p => p.Floor.Description).ToList());
            });
		}

		public override void RowSelected(NSIndexPath path, string val)
		{
            _qualityReportVM.Floor = val;
            //_qualityReportVM.Plan = _qualityReportVM.QualityReport.Plan.First().Key;
            this.Start<QualityReportTasksTableVC>();
		}

        bool hasAppeared;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!hasAppeared)
                hasAppeared = true;
            else
                TableController.ReloadTable();

            var options = new List<(string, Action)>();

            if (userVM.HasPermission(Permission.QualityReport, Permission.CRUDD.Update) && _qualityReportVM.QualityReport.CompletedTime == null)
                options.Add(("Tilføj opgave", () =>
                {
                    AppDelegate.ServiceProvider.GetService<CreateTaskVM>().Location = locationVM.Location;
                    AppDelegate.ServiceProvider.GetService<CreateTaskVM>().QualityReport = _qualityReportVM.QualityReport;
                    //this.Start<CreateCleaningPlansTableVC>();
                    this.Start<TaskCreateVC>().NewTask(locationVM.Location.ID, _qualityReportVM.QualityReport.ID);
                }
                ));
            
            if (userVM.HasPermission(Permission.QualityReport, Permission.CRUDD.Update) && _qualityReportVM.QualityReport.CompletedTime == null)
                options.Add(("Færdiggør kvalitetsrapport", () =>
                {
                    this.Start<QualityReportDateTimeVC>();
                }
                ));

            if (options.Count > 0)
                this.RightNavigationButton("Rediger", (button) => this.DisplayMenu("Kvalitetsrapport muligheder", options, button));
        }
    }
}