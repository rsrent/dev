using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using Microsoft.Extensions.DependencyInjection;
using RentApp.ViewModels;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Navigation;
using System.Linq;
using Rent.Shared.ViewModels;
using CoreGraphics;

namespace RentApp
{
    public partial class CustomersTableVC : ITableAndSourceViewController<Customer>
    {
        CustomerRepository customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        CustomerVM _customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();

        List<Customer> LoadedCustomers = new List<Customer>();
        TableAndSourceController<CustomersTableVC, Customer> TableAndSourceController;

        UISegmentedControl segmentControl;

        string searchText = "";

        public CustomersTableVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableAndSourceController = TableAndSourceController<CustomersTableVC, Customer>.Start(this);

			Title = "Kunder";
			//NavigationController.NavigationBar.Hidden = false;

            if (_userVM.HasPermission(Permission.CUSTOMER, Permission.CRUDD.Create))
            {
                this.RightNavigationButton("Ny", () => {
                    AppDelegate.ServiceProvider.GetService<CustomerVM>().Customer = null;
                    this.Start<AddCustomerVC>().Set_NewCustomer();
                });
            }

            SearchBar.TextChanged += (sender, e) => {
                searchText = SearchBar.Text;
                TableAndSourceController.ReloadTable(MatchingSearch());
            };

            SearchBar.SearchButtonClicked += (sender, e) => {
                SearchBar.ResignFirstResponder();
            };


            segmentControl = new UISegmentedControl();
            segmentControl.Frame = new CGRect(20, 20, 200, 30);

            segmentControl.InsertSegment("Alle", 0, false);
            segmentControl.InsertSegment("Slettede", 1, false);
            segmentControl.SelectedSegment = 0;
            segmentControl.ValueChanged += (sender, e) => {
                var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
                TableAndSourceController.ReloadTable();
            };
            this.NavigationItem.TitleView = segmentControl;
		}

        List<Customer> MatchingSearch()
        {
            if(segmentControl.SelectedSegment == 0)
            {
                return LoadedCustomers.Where(c => ((c.Name != null ? c.Name : "").ToLower().Contains(searchText.ToLower())) && !c.Disabled).ToList();
            }
            else {
                return LoadedCustomers.Where(c => ((c.Name != null ? c.Name : "").ToLower().Contains(searchText.ToLower())) && c.Disabled).ToList();
            }

        }

		public override UITableView GetTable()
		{
			return TableView;
		}

        public override UITableViewCell GetCell(NSIndexPath path, Customer val)
        {
            return TableFunctions.InstanciateCell<CustomersCell>(TableView, "CustomersCell", (cell) => { cell.UpdateCell(val); });
		}

        public override async Task RequestTableData(Action<ICollection<Customer>> updateAction)
        {
			await customerRepository.GetAll((list) => {
                LoadedCustomers = list;
                updateAction.Invoke(MatchingSearch()); 
            });
        }

        public override void RowSelected(NSIndexPath path, Customer val)
        {
            _customerVM.Customer = val;
            this.Start<CustomerMenuVC>();

            SearchBar.ResignFirstResponder();
		}

        bool hasAppeared;
        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            if (!hasAppeared) 
                hasAppeared = true;
            else 
                TableAndSourceController.ReloadTable();
        }
    }
}