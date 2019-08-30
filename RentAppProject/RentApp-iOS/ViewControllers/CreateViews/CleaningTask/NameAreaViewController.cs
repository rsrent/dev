using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using System.Threading.Tasks;
using ModuleLibraryShared.Services;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using System.Collections.Generic;

namespace RentApp
{
    public partial class NameAreaViewController : UIViewController
    {
        private readonly CreateTaskVM _createTaskVM;
		private readonly CleaningTasksRepository cleaningTasksRepository;
		private readonly QualityReportRepository qualityReportRepository;

        public NameAreaViewController (IntPtr handle) : base (handle)
        {
            _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();
			cleaningTasksRepository = AppDelegate.ServiceProvider.GetService<CleaningTasksRepository>();
			qualityReportRepository = AppDelegate.ServiceProvider.GetService<QualityReportRepository>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			Button.TouchUpInside += (sender, e) => {
                _createTaskVM.Task.Comment = TextField.Text;
                _createTaskVM.Task.Floor = _createTaskVM.Floor;
                _createTaskVM.Task.Area = _createTaskVM.Area;



                Func<Task> addTaskFunc = () => cleaningTasksRepository.Add(_createTaskVM.Location.ID, _createTaskVM.Task, (task) =>
                {
                    NavigationController.PopToViewController(_createTaskVM.RootViewController, true);
                    Alert.DisplayToast("Opgave tilføjet", _createTaskVM.RootViewController);
                    /*
                    if (_createTaskVM.QualityReport != null) {
						RateArea(task);
                    } else {
                        NavigationController.PopToViewController(_createTaskVM.RootViewController, true);
						Alert.DisplayToast("Opgave tilføjet", _createTaskVM.RootViewController);
                    } */
                }, () => Alert.DisplayToast("Indlæsningsfejl", this));

                this.DisplayLoadingWhile(addTaskFunc);


				//await new HttpCall.CallManager().Call(HttpCall.CallType.Put, Model.Instance().HttpUri + "CleaningPlans/AddCleaningTask/" + Model.Instance().GetCleaningPlan().ID, content: Task, 
    //            successA: async () => {
				//	//ModelModificationHandler.ReloadCleaningPlan();
					

    //                if(Report != null) {
    //                    await RateArea(Task);
    //                }

				//	NavigationController.PopToViewController(Root, true);
				//	Alert.DisplayToast("Opgave tilføjet", Root);

				//	//TODO if (AsReport) RateArea(areaDetails);
				//	//TODO else AddAnother(areaDetails);
				//}, errorA: () => {
					//Alert.DisplayToast("Indlæsningsfejl", this);
                //});
                //stopAction.Invoke();

				//if (await AddAreaDetails(Task))
				//{
					
				//}
				//else
				//{
					
				//	//Alert.DisplayToast("Error adding task", ViewController);
				//}
			};
		}

		async void RateArea(CleaningTask task)
		{
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            var raiting = 0;

            var options = new List<(string, Action)>();
            options.Add(("Godt", () => { raiting = 1; tcs.SetResult(true); } ));
            options.Add(("Okay", () => { raiting = 2; tcs.SetResult(true); } ));
            options.Add(("Skidt", () => { raiting = 3; tcs.SetResult(true); } ));
            this.DisplayMenu("Hvor rent er " + task.Area.Description, options);

            /*
			Alert.DisplayMenu("Hvor rent er " + task.Area.Description, new[] { "Godt", "Okay", "Skidt" },
                new Action<UIAlertAction>[]
                {  (obj)=> {
                    raiting = 1;
					//AddAnother(areaDetails);
                    tcs.SetResult(true);
				}, (obj) => {
                    raiting = 2;
					//AddAnother(areaDetails);
                    tcs.SetResult(true);
				}, (obj) => {
                    raiting = 3;
					//AddAnother(areaDetails);
                    tcs.SetResult(true);
				}}, this);
*/
            await tcs.Task;

            await qualityReportRepository.AddItem(new QualityReportItem { CleaningTask = task, Rating = raiting, QualityReportID = _createTaskVM.QualityReport.ID, CleaningTaskID = task.ID }, () => {
				NavigationController.PopToViewController(_createTaskVM.RootViewController, true);
				Alert.DisplayToast("Opgave tilføjet", _createTaskVM.RootViewController);
            }, () => Alert.DisplayToast("Indlæsningsfejl", this));
		}

		//async Task<bool> AddAreaDetails(CleaningTask details)
		//{
  //          return await new HttpCall.CallManager().Call(HttpCall.CallType.Put, Model.Instance().HttpUri + "CleaningPlans/AddCleaningTask/" + Model.Instance().GetCleaningPlan().ID, content: details);
		//}
    }
}