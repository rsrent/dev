using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Map;
using System.Linq;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace RentApp
{
    public partial class AddressSearchVC : ITableAndSourceViewController<Address.AddressInfo>
    {
        AddressSearchVM _addressSearchVM;
        TableAndSourceController<AddressSearchVC, Address.AddressInfo> TableController;

        public AddressSearchVC (IntPtr handle) : base (handle)
        {
			_addressSearchVM = AppDelegate.ServiceProvider.GetService<AddressSearchVM>();
        }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableController = TableAndSourceController<AddressSearchVC, Address.AddressInfo>.Start(this);
			SearchBar.BecomeFirstResponder();
            SearchBar.Text = _addressSearchVM.Address;
			SearchBar.TextChanged += (sender, e) => {
                TableController.ReloadTable();
			};
            SearchBar.SearchButtonClicked += (sender, e) => {
                SearchBar.ResignFirstResponder();
            };

            Tabel.TableFooterView = new UIView(new CoreGraphics.CGRect(0, 0, View.Frame.Width, 300));
		}

        public override UITableViewCell GetCell(NSIndexPath path, Address.AddressInfo val)
        {
            return Tabel.StartCell<AddressSearchCell>((cell) => { cell.TextLabel.Text = val.forslagstekst; });
        }

        public override UITableView GetTable() => Tabel;

        public override async Task RequestTableData(Action<ICollection<Address.AddressInfo>> updateAction)
        {
            var results = await Address.SearchAddress(SearchBar.Text);
            //var stringResults = results.Select(a => a.forslagstekst).ToList();
            updateAction.Invoke(results);
        }

        public override void RowSelected(NSIndexPath path, Address.AddressInfo val)
        {
            _addressSearchVM.ItemSeletedAction.Invoke(val);
            SearchBar.Text = val.forslagstekst;
        }
    }
}