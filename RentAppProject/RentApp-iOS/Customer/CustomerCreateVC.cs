using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using ModuleLibraryiOS.Navigation;

namespace RentApp
{
    public partial class CustomerCreateVC : UITableViewController
    {
        CustomerRepository _customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
		Customer _customer;

        public CustomerCreateVC (IntPtr handle) : base (handle) { }


        public override void ViewDidLoad()
        {
            Name.Text = _customer.Name;
            Comment.Text = _customer.Comment;
            KeyAccountManager.Text = _customer.KeyAccountManager?.FirstName;
            CustomerManager.Text = _customer.MainUser?.FirstName;
        }

        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            if(indexPath.Section == 1)
            {
                if(indexPath.Row == 0)
                {
                    this.Start<EmployeeTableVC>().SetLoading_UsersWithRole(Role.HumanResource, (obj) =>
                    {
                        _customer.KeyAccountManagerID = obj.ID;
                    });
                }
                if (indexPath.Row == 1 && _customer.ID > 0)
                {
                    this.Start<UserCreateVC>().NewUser(customerID: _customer.ID, roleID: Role.CustomerManager.ID, completed: async (user) =>
                    {
                        _customer.MainUserID = user.ID;
                        var c = new Customer()
                        {
                            ID = _customer.ID,
                            Comment = _customer.Comment,
                            Name = _customer.Name,
                            MainUserID = _customer.MainUserID,
                            KeyAccountManagerID = _customer.KeyAccountManagerID,
                            ImageLocation = _customer.ImageLocation,
                        };
                        await _customerRepository.UpdateCustomer(_customer, () => {
                            NavigationController.PopViewController(true);
                        });
                    });
                }
            }
        }

        public void NewCustomer()
        {
            _customer = new Customer();
            this.RightNavigationButton("Opret", () => {
                
            });
        }

        public void EditCustomer(Customer customer)
        {
            _customer = customer;
            this.RightNavigationButton("Opdater", () => {

            });
        }

        async Task<Customer> GetCustomer()
        {
            _customer.Name = Name.Text;
            _customer.Comment = Comment.Text;

            return _customer;
        }
    }
}