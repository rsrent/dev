using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Navigation;
using RentApp.Repository;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Image;

namespace RentApp
{
    public partial class SimpleTableVC : ITableAndSourceViewController<(string, string, string)>
    {
        public SimpleTableVC(IntPtr handle) : base(handle)
        {
        }

        public TableAndSourceController<SimpleTableVC, (string, string, string)> TableController;

        string title;
        Action<int> rowSelected;
        ICollection<(string, string, string)> tableData;
        Action<Action<ICollection<(string, string, string)>>> loadTableData;
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        Action<SimpleTableVC> viewPrep;

        public override UITableViewCell GetCell(NSIndexPath path, (string, string, string) val)
        {
            if(string.IsNullOrEmpty(val.Item2))
            {
                return Table.StartCell<SimpleTextCell>(c =>
                {
                    c.TextLabel.Text = val.Item1;

                    if (!string.IsNullOrEmpty(val.Item3))
                    {
                        c.ImageView.Hidden = false;
                        c.ImageView.Image = UIImage.FromBundle("imagePlaceholder");

                        _storage.DownloadImage((img) =>
                        {
                            c.ImageView.Image = img;
                            c.ImageView.Round();
                        }, val.Item3, 50);
                    }
                });
            }
            else 
            {
                return Table.StartCell<SimpleCell>(c =>
                {
                    c.TextLabel.Text = val.Item1;
                    c.DetailTextLabel.Text = val.Item2;

                    if (!string.IsNullOrEmpty(val.Item3))
                    {
                        c.ImageView.Hidden = false;
                        c.ImageView.Image = UIImage.FromBundle("imagePlaceholder");

                        _storage.DownloadImage((img) =>
                        {
                            c.ImageView.Image = img;
                            c.ImageView.Round();
                        }, val.Item3, 50);
                    }
                });
            }
        }

        public override UITableView GetTable()
        => Table;

        public override async Task RequestTableData(Action<ICollection<(string, string, string)>> updateAction)
        {
            if(tableData != null)
                updateAction(tableData);
            loadTableData?.Invoke(updateAction);
        }

        public override void RowSelected(NSIndexPath path, (string, string, string) val)
        {
            rowSelected(path.Row);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableController = TableAndSourceController<SimpleTableVC, (string, string, string)>.Start(this);

            TitelLabel.Text = title;
            viewPrep?.Invoke(this);
        }

        public void Setup(string title, ICollection<(string, string, string)> tableData, Action<int> rowSelected)
        {
            this.title = title;
            this.tableData = tableData;
            this.rowSelected = rowSelected;
        }

        public void Setup(string title, Action<Action<ICollection<(string, string, string)>>> loadTableData, Action<int> rowSelected)
        {
            this.title = title;
            this.loadTableData = loadTableData;
            this.rowSelected = rowSelected;
        }

        public void Reload()
        {
            TableController.ReloadTable();
        }

        public void SetupViewPrep(Action<SimpleTableVC> prep) {
            viewPrep = prep;
        }

        bool hasAppeared;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!hasAppeared)
                hasAppeared = true;
            else
                TableController.ReloadTable();
        }
    }
}