using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using ModuleLibraryiOS.Alert;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using RentApp.ViewModels;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Camera;
using System.Threading.Tasks;
using ModuleLibraryiOS.Image;
using RentApp.Repository;

namespace RentApp
{
    public partial class AddCustomerVC : UIViewController
    {
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        CustomerRepository _customerRepository;

        bool IsNewCustomer;

        Customer Customer;
        NSData imageData;
        //User HRContact;

        public AddCustomerVC(IntPtr handle) : base(handle)
        {
            _customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ImageView.Round();

            if(!IsNewCustomer && Customer != null) {
                NameTF.Text = Customer.Name;
                CommentTV.Text = Customer.Comment;
                LoadImage();



                SelectCustomerResponsible.TouchUpInside += (sender, e) => {


                    CreateCustomerManager(Customer);


                    /*
                    this.Start<AddUserVC>().Set_NewCompanyUser(Customer.ID, Role.CustomerManager,
                    async (User user) =>
                    {
                        Customer.MainUserID = user.ID;
                        await _customerRepository.UpdateCustomer(Customer, CustomerUpdated);
                    }); */
                };

            } else {
                SelectCustomerResponsible.Hidden = true;
            }

            /*

            if (Customer.KeyAccountManager != null)
            {
                SelectHRContact.SetTitle("Key account manager: " + Customer.MainUser.FirstName + " " + Customer.MainUser.LastName, UIControlState.Normal);
            } */

            ImageButton.TouchUpInside += (sender, e) => {
                this.Start<CameraContainerViewController>("Camera").EnableImage((data) =>
                {
                    imageData = data;
                    ImageView.Image = new UIImage(imageData);
                });
            };

            SelectHRContact.TouchUpInside += (sender, e) => {
                this.Start<EmployeeTableVC>().SetLoading_UsersWithRole(Role.HumanResource, (obj) =>
                {
                    //Customer.HRContactID = obj.ID;
                    Customer.KeyAccountManagerID = obj.ID;
                    Customer.KeyAccountManager = obj;
                });
            };
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            TabBarController.TabBar.Hidden = false;

            if(Customer.KeyAccountManager != null)
                SelectHRContact.SetTitle("Key account manager: " + Customer.KeyAccountManager.FirstName, UIControlState.Normal);

            if (Customer.MainUser != null)
                SelectCustomerResponsible.SetTitle("Kundeansvarlig: " + Customer.MainUser.FirstName + " " + Customer.MainUser.LastName, UIControlState.Normal);
        }

        async void LoadImage()
        {
            if (!string.IsNullOrEmpty(Customer.ImageLocation))
            {
                ImageView.Image = await _storage.DownloadImage(Customer.ImageLocation, 200);
            }
        }

        public void Set_NewCustomer()
        {
            IsNewCustomer = true;
            Customer = new Customer();
            Title = "Ny kunde";
            this.RightNavigationButton("Tilføj", async () => {
                var newCustomer = await GetCustomer(Customer);
                if (newCustomer == null)
                    return;

                _customerRepository.AddCustomer(newCustomer, TryAddUserToCustomer).LoadingOverlay(this, "Tilføjer kunde");
            });
        }

		void TryAddUserToCustomer(Customer customer)
		{
            CreateCustomerManager(customer);
            /*
            this.Start<AddUserVC>().Set_NewCompanyUser( customer.ID, Role.CustomerManager,
				async (User user) =>
				{
					customer.MainUserID = user.ID;
					await _customerRepository.UpdateCustomer(customer, CustomerUpdated);
				}); */
		}

		void CustomerUpdated()
		{
			NavigationController.PopToViewController(this, false);
			NavigationController.PopViewController(true);
		}

        void CreateCustomerManager(Customer customer) 
        {
            this.Start<UserCreateVC>().NewUser(customerID: customer.ID, roleID: Role.CustomerManager.ID, completed: async (user) =>
            {
                //customer.MainUserID = user.ID;
                customer.MainUserID = user.ID;
                var c = new Customer()
                {
                    ID = customer.ID,
                    Comment = customer.Comment,
                    Name = customer.Name,
                    MainUserID = customer.MainUserID,
                    KeyAccountManagerID = customer.KeyAccountManagerID,
                    ImageLocation = customer.ImageLocation,
                };
                await Task.Delay(50);
                await _customerRepository.UpdateCustomer(c, success: CustomerUpdated);
            });

            /*
            this.Start<AddUserVC>().Set_NewUser("Kundeansvarlig", (User user) =>
            {
                user.CustomerID = customer.ID;
            }, async (User user) =>
            {
                customer.MainUser = user;
                await _customerRepository.UpdateCustomer(customer, CustomerUpdated);
            }, Role.CustomerManager); */
        }


        public void Set_EditCustomer(Customer customer) 
        {
            Title = customer.Name;
            Customer = customer;
            this.RightNavigationButton("Opdater", async () =>  {
                var customerToUpdate = await GetCustomer(Customer);
                if (customerToUpdate == null)
                    return;

                var c = new Customer()
                {
                    ID = customerToUpdate.ID,
                    Comment = customerToUpdate.Comment,
                    Name = customerToUpdate.Name,
                    MainUserID = customerToUpdate.MainUserID,
                    KeyAccountManagerID = customerToUpdate.KeyAccountManagerID,
                    ImageLocation = customerToUpdate.ImageLocation,
                };

                _customerRepository.UpdateCustomer(c, async () =>
                {
                    Alert.DisplayToast("Kundeinformation opdateret", this);
                    await Task.Delay(500);
                    this.NavigationController.PopViewController(true);
                }).LoadingOverlay(this, "Opdaterer kunde");
            });
        }

        async Task<Customer> GetCustomer(Customer customer = null)
        {
            if (customer == null)
                customer = new Customer();

            customer.Name = NameTF.Text;
            customer.Comment = CommentTV.Text;
            //customer.HRContactID = HRContact.ID;
            //customer.KeyAccountManager = null;

            if (string.IsNullOrEmpty(customer.Name))
            {
                this.DisplayToast("Navn skal udfyldes");
                return null;
            }

            if (imageData != null)
            {
                var imageStream = imageData.AsStream();
                customer.ImageLocation = await _storage.Upload(imageStream, "image", customer.ImageLocation);
            }
            return customer;
        }
    }
}