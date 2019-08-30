using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Foundation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using UIKit;
using ModuleLibraryiOS.Alert;

namespace ModuleLibraryiOS.Table
{
    public class TableAndSourceController<T, Value> where T : ITableAndSourceViewController<Value>
    {
        T viewController;

        public static TableAndSourceController<T, Value> Start(T vc)
        {
            return new TableAndSourceController<T, Value>(vc);
        }

        TableAndSourceController(T vc)
        {
            viewController = vc;
            Start();
        }

        bool useRefreshControl = false;
        UIRefreshControl RefreshControl;

        TableSourceController<T, Value> TableSource;

        private void Start()
        {
            var Table = viewController.GetTable();
            viewController.AutomaticallyAdjustsScrollViewInsets = false;
            TableSource = new TableSourceController<T, Value>(viewController);
            Table.Source = TableSource;
            //Table.RowHeight = UITableView.AutomaticDimension;
            //Table.EstimatedRowHeight = 40f;
            Table.ReloadData();
            AddRefreshControl();
            Table.Add(RefreshControl);

            /*
		    viewController.ParseReloadFunction(async () => {
                await Task.Delay(1);
                viewController.DisplayLoadingWhile(() => ReloadData(() => TableSource.RequestUpdate()), "");
		    });

            viewController.ParseInsertItemsFunction( async () => {
                await TableSource.RequestItemsToInsert();
            });

            viewController.ParseRefreshFunction(async () => {
                await TableSource.RequestRefresh();
            }); */

            Alert.Alert.DisplayLoadingWhile(viewController, RefreshAsync, "");
        }

        public void ReloadTable(List<Value> values = null) {
            if(values == null) {
                viewController.DisplayLoadingWhile(() => ReloadData(() => TableSource.RequestUpdate(values)), "");
            } else {
                ReloadData(() => TableSource.RequestUpdate(values));
            }
        }

        public void ScrollToBottom() => TableSource.ScrollToBottom();

        public void InsertIntoTable(List<Value> values) => TableSource.RequestItemsToInsert(values);

        public void InsertItemsAtIndexes(List<Value> values, NSIndexPath[] paths) => TableSource.InsertItemsAtIndexes(values, paths);
        //public void RefreshTable(List<Value> values) => TableSource.RequestRefresh(values);

        bool refreshing;
		async Task RefreshAsync()
		{
            if(!refreshing) {
				refreshing = true;
				if (useRefreshControl)
					RefreshControl.BeginRefreshing();

				if (useRefreshControl)
					RefreshControl.EndRefreshing();

                await ReloadData(() => TableSource.RequestUpdate());
                refreshing = false;
            }
		}

		async Task ReloadData(Func<Task> updateList)
		{
			if (updateList != null) await updateList.Invoke();
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

	public class TableSourceController<T, Value> : UITableViewSource where T : ITableAndSourceViewController<Value>
	{
        T controller;
		List<Value> list;
        ICollection<(int, Value)> newItems;

		public TableSourceController(T controller)
		{
            this.controller = controller;
			list = new List<Value>();

		}

        public async Task RequestUpdate(List<Value> values = null)
        {
            if (values == null)
            {
                await controller.RequestTableData((newList) =>
                {
                    list = newList.ToList();
                    controller.GetTable().ReloadData();
                });
            }
            else
            {
                list = values;
                controller.GetTable().ReloadData();
            }
        }
        /*
        public async Task RequestRefresh(List<Value> values = null)
        {
            if (values == null)
            {
                await controller.RequestRefreshedData((newList) =>
                {
                    list = newList.ToList();
                    controller.GetTable().ReloadData();
                });
            } else {
                list = values;
                controller.GetTable().ReloadData();
            }
        } */

        public void RequestItemsToInsert(List<Value> values)
        {
            list.AddRange(values);
            controller.GetTable().ReloadData();
        }

        public void InsertItemsAtIndexes(List<Value> values, NSIndexPath[] indexes)
        {
            list = values;
            controller.GetTable().InsertRows(indexes, UITableViewRowAnimation.Left);
        }

        public void ScrollToBottom()
        {
            try
            {
                if (list.Count > 0)
                    controller.GetTable().ScrollToRow(NSIndexPath.FromItemSection(list.Count - 1, 0), UITableViewScrollPosition.None, true);
            }
            catch (Exception exc)
            {
                System.Diagnostics.Debug.WriteLine(exc.Message);
            }
        }

		public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
		{
            return controller.GetCell(indexPath, list.ToList()[indexPath.Row]);
        }

        public override void WillDisplay(UITableView tableView, UITableViewCell cell, NSIndexPath indexPath)
        {
            controller.WillDisplayCell(cell, indexPath, list.ToList()[indexPath.Row]);
        }

		public override nint RowsInSection(UITableView tableview, nint section)
		{
			return list.Count;
		}

		public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
		{
            controller.RowSelected(indexPath, list.ToList()[indexPath.Row]);
            tableView.DeselectRow(indexPath, true);
		}
	}

    public abstract class ITableAndSourceViewController<Value> : UIViewController
    {
        public ITableAndSourceViewController(IntPtr handle) : base(handle) { }
        public abstract UITableView GetTable();
        public abstract UITableViewCell GetCell(NSIndexPath path, Value val);
        public virtual void WillDisplayCell(UITableViewCell cell, NSIndexPath path, Value val) { }
        public virtual void RowSelected(NSIndexPath path, Value val) { }
        public abstract Task RequestTableData(Action<ICollection<Value>> updateAction);
		//public virtual void ParseReloadFunction(Func<Task> Reload) { }

        /*
        public virtual async Task RequestItemsToInsert(Action<ICollection<Value>> updateAction) { }
        public virtual void ParseInsertItemsFunction(Func<Task> Insert) { }
        public virtual async Task RequestRefreshedData(Action<ICollection<Value>> updateAction) { }
        public virtual void ParseRefreshFunction(Func<Task> Refresh) { } */
	}
}
