using System;
using System.Threading.Tasks;
using UIKit;

namespace ModuleLibraryiOS.Table
{
	public class TableTest<T, Value> where T : ITableAndSourceViewController<Value>
	{
        T viewController;

        public static void Start(T vc)
		{
            new TableTest<T, Value>(vc);
        }

        public TableTest(T vc) {
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
			Table.RowHeight = UITableView.AutomaticDimension;
			Table.EstimatedRowHeight = 40f;
			Table.ReloadData();
			AddRefreshControl();
			Table.Add(RefreshControl);

			//ReloadData(true, TableSource.RequestUpdate);
			RefreshAsync();
            /*
			viewController.ParseReloadFunction(async () => {
				//await ReloadData(reloadContent, TableSource.RequestUpdate);
			}); */
		}

		bool refreshing;
		public async Task RefreshAsync()
		{
			if (!refreshing)
			{
				refreshing = true;
				if (useRefreshControl)
					RefreshControl.BeginRefreshing();

				if (useRefreshControl)
					RefreshControl.EndRefreshing();

				//await ReloadData(true, TableSource.RequestUpdate);
				refreshing = false;
			}
		}

		public async Task ReloadData(bool reloadContent, Func<Task> updateList)
		{
			if (updateList != null && reloadContent) await updateList.Invoke();
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
}
