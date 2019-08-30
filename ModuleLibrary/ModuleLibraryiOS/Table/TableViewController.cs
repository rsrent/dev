using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace ModuleLibraryiOS.Table
{
    public partial class TableViewController : ITableViewController
    {

		public TableViewController(IntPtr handle) : base(handle)
        {
		}

        public static TableController<TableViewController> Start(UIViewController vc)
		{
			return new TableController<TableViewController>(vc, "Table", "TableViewController");
		}

		public static TableController<TableViewController> Start(UIViewController vc, UIView container)
		{
			return new TableController<TableViewController>(vc, container, "Table", "TableViewController");
		}

		public override UITableView GetTable()
		{
            return TableView;
		}

        //public override UITableViewSource GetTableSource() {
        //    return Source;
        //}

        //public override Action<UITableView, UIViewController> GetPreperration() {
        //    return Prepersation;
        //}

        //public override Func<Task> GetReloadTableData()
        //{
        //    return ReloadTableData;
        //}

        /*
        bool grouped;
        Action<UITableView, UIViewController> prep;
        UITableViewSource tableSource;

		bool useRefreshControl = false;
		UIRefreshControl RefreshControl;

        Func<Task> RefreshFunc;

		public TableController(IntPtr handle) : base(handle)
        {
        }

		public TableController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }

		public static Func<bool, Task> Start<Value>(UIView container, UIViewController viewController, TableSource<Value> tableSource, Action<UITableView, UIViewController> prep = null, Func<Task> refreshFunc = null)
        {
            return initializer(container, viewController, tableSource, prep, false, refreshFunc);
        }

        public static Func<bool, Task> Start<Key, Value>(UIView container, UIViewController viewController, TableSourceSections<Key, Value> tableSource, Action<UITableView, UIViewController> prep = null, Func<Task> refreshFunc = null)
		{
            return initializer(container, viewController, tableSource, prep, true, refreshFunc);
		}

        private static Func<bool, Task> initializer(UIView container, UIViewController viewController, UITableViewSource tableSource, Action<UITableView, UIViewController> prep, bool grouped, Func<Task> refreshFunc = null) {
			var chatStoryboard = UIStoryboard.FromName("Table", null);
			var newView = chatStoryboard.InstantiateViewController("TableController") as TableController;
            newView.parseInfo(tableSource, prep, grouped, refreshFunc);
			if (container != null && viewController != null)
			{
				newView.View.Frame = container.Bounds;
				newView.WillMoveToParentViewController(viewController);

				container.AddSubview(newView.View);
				viewController.AddChildViewController(newView);
				newView.DidMoveToParentViewController(viewController);
			}
			else if (viewController.NavigationController != null) viewController.NavigationController.PushViewController(newView, true);
			else viewController.PresentViewController(newView, true, null);
			return new Func<bool, Task>(async (bool reloadFromServer) => { await newView.ReloadData(reloadFromServer, refreshFunc); });
        }

        public void Initialize(UITableViewSource source, Action<UITableView, UIViewController> pre, bool grp, Func<Task> refreshFunc = null)
        {
            this.tableSource = source;
            this.prep = pre;
            RefreshFunc = refreshFunc;
            grouped = grp;
        }

        public override void ViewDidLoad()
        {
            AutomaticallyAdjustsScrollViewInsets = false;

            UITableView table = null;
            if (grouped) {
                controller.GetTable().Hidden = true;
                table = TableViewGrouped;
            }
            else {
                TableViewGrouped.Hidden = true;
                table = TableView;
            }

			if (prep != null) prep.Invoke(table, this);
			table.Source = tableSource;
			table.RowHeight = UITableView.AutomaticDimension;
			table.EstimatedRowHeight = 40f;
			table.ReloadData();



            //await RefreshAsync();
			AddRefreshControl();
			table.Add(RefreshControl);
            //RefreshAsync();
            //ReloadData(RefreshFunc);
            //table.Editing = true;
        }

        public static T InstanciateCell<T>(UITableView table, string key, Action<T> UpdateCell) where T : UITableViewCell
        {
            T cell = (T)table.DequeueReusableCell(key);
            if (cell == null) cell = Activator.CreateInstance(typeof(T), new List<object> { key }) as T;
            UpdateCell.Invoke(cell);
            return cell;
        }

        private async Task ReloadData(bool reloadContent, Func<Task> updateList) {

            if(updateList != null && reloadContent) await updateList.Invoke();


            if (TableView != null && !grouped) TableView.ReloadData();
            if (TableViewGrouped != null && grouped) TableViewGrouped.ReloadData();
            await System.Threading.Tasks.Task.Delay(10);
			if (TableView != null && !grouped) TableView.ReloadData();
			if (TableViewGrouped != null && grouped) TableViewGrouped.ReloadData();
        }

		public class TableSource<Value> : UITableViewSource
		{
            Func<UITableView, NSIndexPath, Value, UITableViewCell> getCell;
            Action<int, Value> rowSelected;
            ICollection<Value> list;

            public TableSource(Func<UITableView, NSIndexPath, Value, UITableViewCell> getCell, out Action<ICollection<Value>> reload, Action<int, Value> rowSelected = null)
			{
				this.getCell = getCell;
				this.rowSelected = rowSelected;
                list = new List<Value>();
                reload = new Action<ICollection<Value>>((newList) =>
                {
                    list = newList;
                });
			}

			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
                return getCell.Invoke(tableView, indexPath, list.ToList()[indexPath.Row]);
			}

            public override nint RowsInSection(UITableView tableview, nint section) {
                return list.Count();
            }

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
                if (rowSelected != null) rowSelected.Invoke(indexPath.Row, list.ToList()[indexPath.Row]);
				tableView.DeselectRow(indexPath, true);
			}


			//public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
			//{
			//	switch (editingStyle)
			//	{
			//		case UITableViewCellEditingStyle.Insert:
			//			//tableItems.Insert(indexPath.Row, new TableItem("(inserted)"));
			//			tableView.InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
			//			break;

			//		case UITableViewCellEditingStyle.None:
			//			Console.WriteLine("CommitEditingStyle:None called");
			//			break;
			//	}
			//}

			//public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
			//{
			//	return true;
			//}

			//public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
			//{
			//	if (tableView.Editing)
			//	{
			//		if (indexPath.Row == tableView.NumberOfRowsInSection(0) - 1)
			//			return UITableViewCellEditingStyle.Insert;
			//		else
			//			return UITableViewCellEditingStyle.Delete;
			//	}
			//	else  // not in editing mode, enable swipe-to-delete for all rows  
			//		return UITableViewCellEditingStyle.Delete;
			//}

			//public void WillBeginTableEditing(UITableView tableView)
			//{
			//	tableView.BeginUpdates();

			//	tableView.InsertRows(new NSIndexPath[] {
			//		NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
			//	}, UITableViewRowAnimation.Fade);
			//	//tableItems.Add(new TableItem("(add new)"));

			//	tableView.EndUpdates();
			//} 
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
   //         
			//public override string[] SectionIndexTitles(UITableView tableView)
			//{
   //             return dir.Keys.ToArray();
   //             //return getSectionTitles.Invoke();
			//}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
                return dir[dir.Keys.ToArray()[section]].Count();
			}

			public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
			{
                if(rowSelected != null) rowSelected.Invoke(indexPath.Row, dir[dir.Keys.ToArray()[indexPath.Section]][indexPath.Row]);
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

            ReloadData(true, RefreshFunc);
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
		} */
        
    }

    //public abstract class ITableViewController : IStartable
    //{
    //	public ITableViewController(IntPtr handle) : base(handle) { }

    //       public abstract UITableView GetTable();
    //}
}