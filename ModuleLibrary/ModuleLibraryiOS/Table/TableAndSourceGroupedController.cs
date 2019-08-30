using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.Alert;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public class TableAndSourceGroupedController<T, Value> where T : ITableAndSourceGroupedViewController<Value>
	//public class TableAndSourceGroupedController<T, Value> : IStarter<T> where T : ITableAndSourceGroupedViewController<Value>
	{
        /*
		public TableAndSourceGroupedController(IntPtr handle) : base(handle) { }
		public TableAndSourceGroupedController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }
		public TableAndSourceGroupedController(UIViewController viewController, UIView container, string storyBoard, string identifier) : base(viewController, container, storyBoard, identifier) { }
*/
        T viewController;

		public static TableAndSourceGroupedController<T, Value> Start(T vc)
		{
			return new TableAndSourceGroupedController<T, Value>(vc);
		}

		public TableAndSourceGroupedController(T vc)
		{
			viewController = vc;
			Start();
		}

		bool useRefreshControl = false;
		UIRefreshControl RefreshControl;

		TableGroupedSourceController<T, Value> TableSource;

		private void Start()
		{
			//base.ViewDidLoad();
			var Table = viewController.GetTable();
			viewController.AutomaticallyAdjustsScrollViewInsets = false;
			TableSource = new TableGroupedSourceController<T, Value>(viewController);
			Table.Source = TableSource;
			Table.RowHeight = UITableView.AutomaticDimension;
			Table.EstimatedRowHeight = 40f;
			Table.ReloadData();
			AddRefreshControl();
			Table.Add(RefreshControl);

            //RefreshAsync();
			//ReloadData(true, TableSource.RequestUpdate);

			viewController.ParseReloadFunction(async (reloadContent) => {
                await Task.Delay(1);
                viewController.DisplayLoadingWhile(() => ReloadData(reloadContent, () => TableSource.RequestUpdate()), "");
			});

            Alert.Alert.DisplayLoadingWhile(viewController, RefreshAsync, "");
		}

		async Task RefreshAsync()
		{
			if (useRefreshControl)
				RefreshControl.BeginRefreshing();

			if (useRefreshControl)
				RefreshControl.EndRefreshing();

			await ReloadData(true, TableSource.RequestUpdate);
		}

		private async Task ReloadData(bool reloadContent, Func<Task> updateList)
		{
            if (updateList != null && reloadContent) await updateList.Invoke();
            /*
			if (updateList != null && reloadContent) await updateList.Invoke();
			if (viewController.GetTable() != null) viewController.GetTable().ReloadData();
			await Task.Delay(10);
			if (viewController.GetTable() != null) viewController.GetTable().ReloadData();
			*/
		}

		void AddRefreshControl()
		{
			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				RefreshControl = new UIRefreshControl();
				RefreshControl.ValueChanged += async (sender, e) =>
				{
					await RefreshAsync();
				};
				useRefreshControl = true;
			}
		}
	}

	public class TableGroupedSourceController<T, Value> : UITableViewSource where T : ITableAndSourceGroupedViewController<Value>
	{
		T controller;
        Dictionary<string, List<Value>> dir = new Dictionary<string, List<Value>>();

		public TableGroupedSourceController(T controller)
		{
			this.controller = controller;
		}

		public async Task RequestUpdate()
		{
			await controller.RequestTableData((newDir) =>
			{
				dir = newDir;
                controller.GetTable().ReloadData();
			});
		}

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
			return controller.GetCell(indexPath, dir[dir.Keys.ToArray()[indexPath.Section]][indexPath.Row]);
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
			controller.RowSelected(indexPath, dir[dir.Keys.ToArray()[indexPath.Section]][indexPath.Row]);
            tableView.DeselectRow(indexPath, true);
		}

		public override string TitleForHeader(UITableView tableView, nint section)
		{
			return dir.Keys.ToArray()[section] as string;
		}
	}

	public abstract class ITableAndSourceGroupedViewController<Value> : UIViewController
	{
		public ITableAndSourceGroupedViewController(IntPtr handle) : base(handle) { }
		public abstract UITableView GetTable();
		public abstract UITableViewCell GetCell(NSIndexPath path, Value val);
        public virtual void RowSelected(NSIndexPath path, Value val) { }
		public abstract Task RequestTableData(Action<Dictionary<string, List<Value>>> updateAction);
        public virtual void ParseReloadFunction(Func<bool, Task> Reload) { }
	}
}
