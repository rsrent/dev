using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using ModuleLibraryiOS.Alert;
using System.Linq;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;

namespace RentApp
{
    public partial class QualityReportTasksTableVC : ITableAndSourceViewController<QualityReportItem>
    {
        QualityReportVM _qualityReportVM = AppDelegate.ServiceProvider.GetService<QualityReportVM>();

        public QualityReportTasksTableVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableAndSourceController<QualityReportTasksTableVC, QualityReportItem>.Start(this);
            Title = "";
			NavigationController.NavigationBar.Hidden = false;
            TitleLabel.Text = "Opgaver til " + _qualityReportVM.Floor.ToLower();
		}

		public override UITableViewCell GetCell(NSIndexPath path, QualityReportItem val)
		{
            //QualityReportTasksTestCell
            return TableFunctions.InstanciateCell<QualityReportTasksTestCell>(TableView, "QualityReportTasksTestCell", (cell) => { cell.UpdateCell(val, this); });
            //return TableFunctions.InstanciateCell<QualityReportTasksCell>(TableView, "QualityReportTasksCell", (cell) => { cell.UpdateCell(val, this); });
		}

		public override UITableView GetTable()
        {
            return TableView;
        }

        public override async Task RequestTableData(Action<ICollection<QualityReportItem>> updateAction)
		{
            await qualityReportRepository.Get(_qualityReportVM.QualityReport.ID, (qualityReport) =>
            {
                _qualityReportVM.QualityReport = qualityReport;

                var list = new List<QualityReportItem>();

                foreach (var a in qualityReport.Floors.FirstOrDefault(f => f.Floor.Description == _qualityReportVM.Floor).Areas)
                {
                    list.AddRange(a.QualityReportItems);
                }

                updateAction.Invoke(list);
                //updateAction.Invoke(qualityReport.Plan[_qualityReportVM.Plan][_qualityReportVM.Floor]);
            }, () => Alert.DisplayToast("Indlæsningsfejl", this));
		}

		public override void RowSelected(NSIndexPath path, QualityReportItem val)
		{
			string raiting = val.Rating == 1 ? "Good" : val.Rating == 2 ? "Okay" : val.Rating == 3 ? "Bad" : "Unrated";
			string description = val.CleaningTask.Floor.Description + ", " + val.CleaningTask.Area.Description.ToLower() + " " + val.CleaningTask.SquareMeters + "m²\n";
			if (val.CleaningTask.Frequency != null) description += "Cleaned : " + val.CleaningTask.Frequency;
            else if (val.CleaningTask.TimesOfYear != null) description += "Cleaned : " + val.CleaningTask.TimesOfYear + " times yearly";

			description += "\n\n" + (string.IsNullOrEmpty(val.Comment) ? "No comment" : "Comment:\n" + val.Comment);

			Alert.DisplayInfo(val.CleaningTask.Comment + " - " + raiting, description, this);
		}

        QualityReportRepository qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();

    }
}