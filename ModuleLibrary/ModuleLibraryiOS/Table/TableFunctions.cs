using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public static class TableFunctions
    {
		public static C InstanciateCell<C>(UITableView table, string key, Action<C> UpdateCell) where C : UITableViewCell
		{
			C cell = (C)table.DequeueReusableCell(key);
			if (cell == null) cell = Activator.CreateInstance(typeof(C), new List<object> { UITableViewCellStyle.Default, key }) as C;
			UpdateCell.Invoke(cell);
			return cell;
		}

		public static C StartCell<C>(this UITableView table, Action<C> UpdateCell) where C : UITableViewCell
		{
            var key = typeof(C).ToString().Split('.').Last();
			C cell = (C)table.DequeueReusableCell(key);
			if (cell == null) cell = Activator.CreateInstance(typeof(C), new List<object> { UITableViewCellStyle.Default, key }) as C;
			UpdateCell.Invoke(cell);
			return cell;
		}

        public static C InstanciateCell<C>(UITableView table, string identifier) where C : UITableViewCell {
			C cell = table.DequeueReusableCell(identifier) as C;
			//string item = TableItems[indexPath.Row];

			//---- if there are no cells to reuse, create a new one
			if (cell == null)
            { 
                cell = Activator.CreateInstance(typeof(C), new List<object> { UITableViewCellStyle.Default, identifier }) as C; 
            }

			//cell.TextLabel.Text = item;

			return cell;
        }

		public static UIRefreshControl AddRefreshControl(Func<Task> RefreshAsync)
		{
            UIRefreshControl RefreshControl = null;

			if (UIDevice.CurrentDevice.CheckSystemVersion(6, 0))
			{
				RefreshControl = new UIRefreshControl();
				RefreshControl.ValueChanged += async (sender, e) =>
				{
					RefreshAsync();
				};
				//useRefreshControl = true;
			}

            return RefreshControl;
		}
    }
}
