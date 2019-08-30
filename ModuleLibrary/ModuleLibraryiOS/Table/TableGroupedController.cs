using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    /*
    public class TableGroupedController<T> : IStarter<T> where T : ITableGroupedViewController
    {
		public TableGroupedController(IntPtr handle) : base(handle) { }
		public TableGroupedController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }

		Action<UITableView, UIViewController> prep;
		UITableViewSource tableSource;

		bool useRefreshControl = false;
		UIRefreshControl RefreshControl;

		Func<Task> RefreshFunc;

		public void Initialize(UITableViewSource source, Action<UITableView, UIViewController> pre, Func<Task> refreshFunc = null)
		{
			this.tableSource = source;
			this.prep = pre;
			RefreshFunc = refreshFunc;
		}

		public override void ViewDidLoad()
        {
            AutomaticallyAdjustsScrollViewInsets = false;
            UITableView table = controller.GetTable();
			if (prep != null) prep.Invoke(table, this);
			table.Source = tableSource;
			table.RowHeight = UITableView.AutomaticDimension;
			table.EstimatedRowHeight = 40f;
			table.ReloadData();
			RefreshControl = TableFunctions.AddRefreshControl(RefreshAsync);

			//AddRefreshControl();
			table.Add(RefreshControl);
        }

        public static C InstanciateCell<C>(UITableView table, string key, Action<C> UpdateCell) where C : UITableViewCell
		{
			C cell = (C)table.DequeueReusableCell(key);
			if (cell == null) cell = Activator.CreateInstance(typeof(C), new List<object> { key }) as C;
			UpdateCell.Invoke(cell);
			return cell;
		}

		private async Task ReloadData(bool reloadContent, Func<Task> updateList)
		{
			if (updateList != null && reloadContent) await updateList.Invoke();
            if(controller.GetTable() != null)controller.GetTable().ReloadData();
            await Task.Delay(10);
			if (controller.GetTable() != null) controller.GetTable().ReloadData();
		}

		public class TableSourceSections<Key, Value> : UITableViewSource
		{
			Func<UITableView, NSIndexPath, Value, UITableViewCell> getCell;
			Action<int, Value> rowSelected;
			Dictionary<Key, List<Value>> dir;

			public TableSourceSections(Func<UITableView, NSIndexPath, Value, UITableViewCell> getCell, Dictionary<Key, List<Value>> dir, Action<int, Value> rowSelected = null)
			{
				this.getCell = getCell;
				this.rowSelected = rowSelected;
				this.dir = dir;
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				return getCell.Invoke(tableView, indexPath, dir[dir.Keys.ToArray()[indexPath.Section]][indexPath.Row]);
			}

			public override nint NumberOfSections(UITableView tableView)
			{
				return dir.Keys.Count;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return dir[dir.Keys.ToArray()[section]].Count();
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
				if (rowSelected != null) rowSelected.Invoke(indexPath.Row, dir[dir.Keys.ToArray()[indexPath.Section]][indexPath.Row]);
				tableView.DeselectRow(indexPath, true);
			}

			public override string TitleForHeader(UITableView tableView, nint section)
			{
				return dir.Keys.ToArray()[section] as string;
			}
		}

		async Task RefreshAsync()
		{
			if (useRefreshControl)
				RefreshControl.BeginRefreshing();

			if (useRefreshControl)
				RefreshControl.EndRefreshing();

			await ReloadData(true, RefreshFunc);
		}

		//void AddRefreshControl()
		//{
		//	if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
		//	{
		//		RefreshControl = new UIRefreshControl();
		//		RefreshControl.ValueChanged += async (sender, e) =>
		//		{
		//			await RefreshAsync();
		//		};
		//		useRefreshControl = true;
		//	}
		//}
    }

    public abstract class ITableGroupedViewController : IStartable
	{
		public ITableGroupedViewController(IntPtr handle) : base(handle) { }

		public abstract UITableView GetTable();
	}*/
}
