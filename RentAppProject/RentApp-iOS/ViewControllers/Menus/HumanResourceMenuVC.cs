using System;
using RentApp.ViewModels;
using UIKit;
using Microsoft.Extensions.DependencyInjection;
using RentAppProject;
using ModuleLibraryiOS.ViewControllerInstanciater;

namespace RentApp
{
    public partial class HumanResourceMenuVC : UIViewController
    {
		public HumanResourceMenuVC(IntPtr handle) : base(handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			NavigationController.NavigationBar.Hidden = false;

            /*
			AddNewEmployeeButton.TouchUpInside += (sender, e) => {

				this.Start<AddUserVC>().Set_NewEmployeeUser( async (User user) => {
					this.NavigationController.PopToViewController(this, true);
                });

    //            var createUserVM = AppDelegate.ServiceProvider.GetService<CreateUserVM>();
    //            createUserVM.DoneAction = async (User user) => {
    //                this.NavigationController.PopToViewController(this, true);
    //            };
				//createUserVM.NewUser = new User { UserRole = "" }; 
			};

			AddNewCustomerButton.TouchUpInside += (sender, e) => {
				AppDelegate.ServiceProvider.GetService<CustomerVM>().Customer = null;
                this.Start<AddCustomerVC>().Set_NewCustomer();
			};
*/

            ViewEmployeesButton.TouchUpInside += (sender, e) => {
                this.Start<EmployeeTableVC>().SetLoadSource_AllUsers();
            };
		} // Release any cached data, images, etc that aren't in use.
    }
}

