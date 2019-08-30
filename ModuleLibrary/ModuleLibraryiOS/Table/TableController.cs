using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public class TableController<T> : IStarter<T> where T : ITableViewController
    {
        public TableController(IntPtr handle) : base(handle) { }
        public TableController(UIViewController viewController, string storyBoard, string identifier) : base(viewController, storyBoard, identifier) { }
        public TableController(UIViewController viewController, UIView container, string storyBoard, string identifier) : base(viewController, container, storyBoard, identifier) { }

        Action<UITableView, UIViewController> Preperation;
		UITableViewSource TableSource;

		bool useRefreshControl = false;
		UIRefreshControl RefreshControl;

        Func<Task> ReloadTableData;

        UITableView Table;

		public TableController<T> Initialize(UITableViewSource source, Action<UITableView, UIViewController> pre, Func<Task> reloadTableData = null)
		{
			this.TableSource = source;
			this.Preperation = pre;
			ReloadTableData = reloadTableData;
            return this;
		}

		public TableController<T> Initialize(UITableViewSource source)
		{
			this.TableSource = source;
			return this;
		}

		public override void ViewDidLoad()
		{
            //if (tableSource == null) 
            //    tableSource = viewController.GetTableSource();
            //if (Preperation == null)
            //    Preperation = viewController.GetPreperration();
            //if (ReloadTableData == null)
            //ReloadTableData = viewController.GetReloadTableData();

            if (TableSource != null)
                SetTable();
            if(Preperation != null)
                Preperation.Invoke(Table, viewController);
            

			//AutomaticallyAdjustsScrollViewInsets = false;
			//Table = viewController.GetTable();
   //         if (Preperation != null) Preperation.Invoke(Table, viewController);
			//Table.Source = TableSource;
			//Table.RowHeight = UITableView.AutomaticDimension;
			//Table.EstimatedRowHeight = 40f;
			//Table.ReloadData();
   //         RefreshControl = TableFunctions.AddRefreshControl(RefreshAsync);
			//Table.Add(RefreshControl);
		}

        public Func<bool, Task> GetRefreshFunction() {
            return new Func<bool, Task>(async (bool reloadFromServer) => { await ReloadData(reloadFromServer, ReloadTableData); });
        }

		private async Task ReloadData(bool reloadContent, Func<Task> updateList)
		{
			if (updateList != null && reloadContent) await updateList.Invoke();
			if (viewController.GetTable() != null) viewController.GetTable().ReloadData();
			await Task.Delay(10);
			if (viewController.GetTable() != null) viewController.GetTable().ReloadData();
		}

		async Task RefreshAsync()
		{
			if (useRefreshControl)
				RefreshControl.BeginRefreshing();

			if (useRefreshControl)
				RefreshControl.EndRefreshing();

			await ReloadData(true, ReloadTableData);
		}

		public void SetTableSource(UITableViewSource source)
		{
            TableSource = source;
            if(ViewLoaded) {
                SetTable();
            }
		}

		public virtual void SetPreperation(Action<UITableView, UIViewController> preperation)
		{
			Preperation = preperation;
            if(ViewLoaded) {
                Preperation.Invoke(Table, viewController);
            }
		}

		public virtual void SetReloadTableData(Func<Task> func)
		{
			ReloadTableData = func;
		}

        void SetTable() {
			AutomaticallyAdjustsScrollViewInsets = false;
			Table = viewController.GetTable();
			Table.Source = TableSource;
			Table.RowHeight = UITableView.AutomaticDimension;
			Table.EstimatedRowHeight = 40f;
			Table.ReloadData();
			AddRefreshControl();
			Table.Add(RefreshControl);
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

    public abstract class ITableViewController : IStartable
	{
		public ITableViewController(IntPtr handle) : base(handle) { }

		public abstract UITableView GetTable();

        public UITableViewSource Source;
        /*
        public abstract UITableViewSource GetTableSource();
		public virtual void SetTableSource(UITableViewSource source)
		{
			Source = source;
		}

		public Action<UITableView, UIViewController> Prepersation;
		public abstract Action<UITableView, UIViewController> GetPreperration();
		public virtual void SetPreperation(Action<UITableView, UIViewController> preperation)
		{
			Prepersation = preperation;
		}

        public Func<Task> ReloadTableData;
		public abstract Func<Task> GetReloadTableData();
		public virtual void SetReloadTableData(Func<Task> func)
		{
            ReloadTableData = func;
		}*/
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

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return list.Count();
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
			if (rowSelected != null) rowSelected.Invoke(indexPath.Row, list.ToList()[indexPath.Row]);
			tableView.DeselectRow(indexPath, true);
		}
		/*
		public override void CommitEditingStyle(UITableView tableView, UITableViewCellEditingStyle editingStyle, NSIndexPath indexPath)
		{
			switch (editingStyle)
			{
				case UITableViewCellEditingStyle.Insert:
					//tableItems.Insert(indexPath.Row, new TableItem("(inserted)"));
					tableView.InsertRows(new NSIndexPath[] { indexPath }, UITableViewRowAnimation.Fade);
					break;

				case UITableViewCellEditingStyle.None:
					Console.WriteLine("CommitEditingStyle:None called");
					break;
			}
		}

		public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow(UITableView tableView, NSIndexPath indexPath)
		{
			if (tableView.Editing)
			{
				if (indexPath.Row == tableView.NumberOfRowsInSection(0) - 1)
					return UITableViewCellEditingStyle.Insert;
				else
					return UITableViewCellEditingStyle.Delete;
			}
			else  // not in editing mode, enable swipe-to-delete for all rows  
				return UITableViewCellEditingStyle.Delete;
		}

		public void WillBeginTableEditing(UITableView tableView)
		{
			tableView.BeginUpdates();

			tableView.InsertRows(new NSIndexPath[] {
				NSIndexPath.FromRowSection (tableView.NumberOfRowsInSection (0), 0)
			}, UITableViewRowAnimation.Fade);
			//tableItems.Add(new TableItem("(add new)"));

			tableView.EndUpdates();
		} */
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

}
