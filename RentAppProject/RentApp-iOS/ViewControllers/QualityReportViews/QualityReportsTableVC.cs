using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Services;
using System.Linq;
using ModuleLibraryiOS.Alert;
using ModuleLibraryShared.Services;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class QualityReportsTableVC : ITableAndSourceViewController<QualityReport>
    {
		QualityReportRepository qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
        UserVM userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        LocationVM _locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();
        QualityReportVM _qualityReportVM = AppDelegate.ServiceProvider.GetService<QualityReportVM>();

        public QualityReportsTableVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            var TableController = TableAndSourceController<QualityReportsTableVC, QualityReport>.Start(this);

			Title = "";
            TitleLabel.Text = "Kvalitetsrapporter";

			NavigationController.NavigationBar.Hidden = false;

			if (userVM.HasPermission(Permission.QualityReport, Permission.CRUDD.Create))
			{
                this.RightNavigationButton("Ny", () =>
                {
                    qualityReportRepository.Create(_locationVM.Location.ID, (obj) =>
                    {
                        TableController.ReloadTable();
                        _qualityReportVM.QualityReport = obj;
                        this.Start<QualityReportFloorsTableVC>();
                    }).LoadingOverlay(this, "Laver ny kvalitetsrapport");
                });
			}
		}

		public override UITableViewCell GetCell(NSIndexPath path, QualityReport val)
		{
            return TableFunctions.InstanciateCell<QualityReportsCell>(TableView, "QualityReportsCell", (cell) => { 
                cell.TextLabel.Text = val.Time.ToString("dd/MM/yy");
                if (val.CompletedTime != null)
                    cell.TextLabel.TextColor = UIColor.FromName("FadeTextColor");
                else
                    cell.TextLabel.TextColor = UIColor.Black;
            });
		}

		public override UITableView GetTable()
        {
            return TableView;
        }

        public override async Task RequestTableData(Action<ICollection<QualityReport>> updateAction)
		{
			await qualityReportRepository.GetMany(_locationVM.Location.ID, (reportList) =>
			{
				updateAction.Invoke(reportList);
			});
		}

		public override async void RowSelected(NSIndexPath path, QualityReport val)
		{
            _qualityReportVM.QualityReport = val;
            this.Start<QualityReportFloorsTableVC>();
            //TODO FIX THIS

            //QualityReportFloorsTableVC.Start(this).ParseInfo(Location, val);


			//QualityReportPlansTableVC.Start(this, val);

			//await ModelModificationHandler.LoadQualityReport(val.ID, this, (report) =>
			//{
   //             QualityReportFloorsTableVC.Start(this).ParseInfo(val, report.AsDictionary(Model.Instance().GetCleaningPlan()).First().Key);
			//}, () =>
			//{
			//	Alert.DisplayToast("Indlæsningsfejl", this);
			//});

            //QualityReportFloorsTableVC.Start(this, val, val.AsDictionary(Model.Instance().GetCleaningPlan()).First().Key);
		}
    }
}