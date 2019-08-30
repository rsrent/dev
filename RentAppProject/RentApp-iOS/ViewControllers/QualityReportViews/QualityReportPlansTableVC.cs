using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Threading.Tasks;
using System.Collections.Generic;
using ModuleLibraryShared.Services;
using System.Linq;
using ModuleLibraryiOS.Alert;
using RentAppProject;

namespace RentApp
{
	//public partial class QualityReportPlansTableVC : ITableAndSourceViewController<string>, IModuleObserver<Model>
	//{
		//public QualityReportPlansTableVC(IntPtr handle) : base (handle)
  //      {
		//}

		//public static QualityReportPlansTableVC Start(UIViewController vc)
		//{
		//	return new TableAndSourceController<QualityReportPlansTableVC, string>(vc, "Main", "QualityReportPlansTableVC").GetViewController();
		//}

  //      QualityReport QualityReport;
  //      public void ParseInfo(QualityReport qualityReport) {
  //          QualityReport = qualityReport;
  //      }

		//public override UITableViewCell GetCell(NSIndexPath path, string val)
		//{
  //          return TableFunctions.InstanciateCell<QualityReportPlansCell>(TableView, "QualityReportPlansCell", (cell) => { cell.TextLabel.Text = val; });
		//}

		//public override UITableView GetTable()
		//{
		//	return TableView;
		//}

		//public void ObservableUpdated(Model model)
		//{
			
		//}

		//public override async Task RequestTableData(Action<ICollection<string>> updateAction)
		//{
  //          await new QualityReportRepository().Get(QualityReport.ID, (report) =>
  //          {
		//		QualityReport = report;
  //              updateAction.Invoke(report.Plan.Keys);
  //          }, () =>
  //          {
  //              Alert.DisplayToast("Indlæsningsfejl", this);
  //          });

  //          //await ModelModificationHandler.LoadQualityReport(QualityReport.ID, this, (report) =>
  //          //{
  //          //    QualityReport = report;
  //          //    updateAction.Invoke(report.AsDictionary(Model.Instance().GetCleaningPlan()).Keys);
  //          //}, () =>
  //          //{
  //          //    Alert.DisplayToast("Indlæsningsfejl", this);
  //          //});
		//}

		//public override void RowSelected(NSIndexPath path, string val)
		//{
  //          if (QualityReport.AsDictionary(Model.Instance().GetCleaningPlan())[val].Count == 1)
		//	{
  //              QualityReportTasksTableVC.Start(this, QualityReport, val, QualityReport.AsDictionary(Model.Instance().GetCleaningPlan())[val].First().Key);
		//	}
		//	else
		//	{
  //              QualityReportFloorsTableVC.Start(this).ParseInfo(QualityReport, val);
		//	}
		//}

		//IDisposable ModelDisposer;

  //      public override void ViewDidLoad()
  //      {
  //          base.ViewDidLoad();
		//	ModelDisposer = Model.Instance().Subscribe(this);
		//	Title = QualityReport.Time.ToString("dd/MM/yy");
		//	NavigationController.NavigationBar.Hidden = false;
		//	if (Model.Instance().HasPermission(User.Permission.AddCleaningTask))
		//	{
		//		NavigationItem.SetRightBarButtonItem(new UIBarButtonItem("Add", UIBarButtonItemStyle.Plain, (sender, e) =>
		//		{
		//			FloorAreaPickerViewController.Start(this).ParseInfo(QualityReport);
		//		}), true);
		//	}
  //      }

		//public override void ViewDidDisappear(bool animated)
		//{
		//	base.ViewDidDisappear(animated);
  //          if(NavigationController == null || !NavigationController.ViewControllers.Contains(this))
  //              ModelDisposer.Dispose();
		//}
    //}
}