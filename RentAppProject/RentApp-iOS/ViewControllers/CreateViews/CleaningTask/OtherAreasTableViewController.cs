using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace RentApp
{
    public partial class OtherAreasTableViewController : ITableAndSourceViewController<CleaningPlan>
    {
        private readonly CreateTaskVM _createTaskVM = AppDelegate.ServiceProvider.GetService<CreateTaskVM>();

        public OtherAreasTableViewController (IntPtr handle) : base (handle) { }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableAndSourceController<OtherAreasTableViewController, CleaningPlan>.Start(this);
        }

        public override UITableViewCell GetCell(NSIndexPath path, CleaningPlan val)
        {
            return TableView.StartCell<OtherAreasCell>((cell) => { cell.TextLabel.Text = val.Description; });
		}

        public override UITableView GetTable() 
        { 
            return TableView; 
        }

        public override async Task RequestTableData(Action<ICollection<CleaningPlan>> updateAction)
        {
            updateAction.Invoke(new CleaningPlan[] 
            { 
                new CleaningPlan { Description = "Vinduer", ID = 2 }, 
                new CleaningPlan { Description = "Fan Coil", ID = 3 } });
        }

        public override void RowSelected(NSIndexPath path, CleaningPlan val)
        {
            _createTaskVM.Task.Area.CleaningPlanID = val.ID;
            _createTaskVM.Task.Floor = new Floor { Description = val.Description, ID = val.ID };

        }
    }
}