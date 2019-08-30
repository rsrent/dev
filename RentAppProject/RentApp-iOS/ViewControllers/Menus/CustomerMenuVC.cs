using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Image;
using System.Collections.Generic;
using RentApp.Repository;
using Rent.Shared.ViewModels;
using System.Linq;

namespace RentApp
{
    public partial class CustomerMenuVC : UIViewController
    {

        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        CustomerVM _customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        CustomerRepository _customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();


        public CustomerMenuVC (IntPtr handle) : base (handle) { }

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            ImageView.Round();
            SetDetails();

            Title = _customerVM.Customer.Name;
            this.AddNavigationStack();

            ViewLocationsButton.TouchUpInside += (sender, e) => {
                this.Start<LocationTableVC>().Set_ViewForCustomer(_customerVM.Customer);
            };

            DocumentsButton.TouchUpInside += (sender, e) => {
                this.Start<DocumentsTableVC>().ParseInfo(null, _customerVM.Customer.ID, null);
            };

            EconomyButton.Hidden = true;
            EconomyButton.TouchUpInside += (sender, e) => {
                this.Start<LocationEconomyVC>().Setup(null, _customerVM.Customer.ID);
            };

            if (_userVM.HasPermission(Permission.CUSTOMER, Permission.CRUDD.Update))
            {
                var options = new List<(string, Action)>();

                if (_userVM.HasPermission("Customer", Permission.CRUDD.Update))
                    options.Add(("Rediger kunde", () => this.Start<AddCustomerVC>().Set_EditCustomer(_customerVM.Customer)));

                if (_userVM.HasPermission("Customer", Permission.CRUDD.Update))
                    options.Add(("Inviter kunde", inviteCustomer));

                /*
                if (_userVM.HasPermission("Customer", Permission.CRUDD.Delete))
                    options.Add(("Slet kunde", () => {
                    this.DisplayAlert("Slet kunde", "Du er ved at slette denne kunde, er du sikker på at du vil fortsætte?", new List<(string, Action)>()
                        {
                            ("Ja", async () => { await _customerRepository.Delete(_customerVM.Customer.ID, () => {
                                this.NavigationController.PopViewController(true);
                            });
                            }),
                            ("Nej", () => {  })
                        });
                    }
                    ));
                    */
                if (_userVM.HasPermission("Customer", Permission.CRUDD.Delete))
                    DeleteCustomerOption(options);

                if (options.Count > 0)
                    this.RightNavigationButton("Rediger", (button) => this.DisplayMenu(_customerVM.Customer.Name, options, button));

                /*
                Alert.DisplayMenu(location.Name, new List<(string, Action)> {
                    ("Rediger lokation", () => { this.Start<AddLocationVC>().Set_EditLocation(_customerVM.Customer, _locationVM.Location); }),
                    ("Slet lokalitet", () => { 

                        this.DisplayAlert("Slet lokalitet", "Du er ved at slette denne lokalitet, er du sikker på at du vil fortsætte?", new List<(string, Action)>()
                        {
                            ("Ja", async () => { await _locationRepository.DeleteLocation(_locationVM.Location.ID, () => {
                                this.NavigationController.PopViewController(true);
                            });
                            }),
                            ("Nej", () => {  })
                        });
                    })
                }, this);
*/


                if (!_userVM.HasPermission(Permission.ECONOMY, Permission.CRUDD.Read)) EconomyButton.Hidden = true;
                else EconomyButton.Hidden = false;




                /*

                this.RightNavigationButton("Rediger", () => {
                    Alert.DisplayMenu(_customerVM.Customer.Name, new List<(string, Action)> {
                        ("Rediger kunde", () => { this.Start<AddCustomerVC>().Set_EditCustomer(_customerVM.Customer); }),
                        ("Slet kunde", () => { 

                            this.DisplayAlert("Slet kunde", "Du er ved at slette denne kunde, er du sikker på at du vil fortsætte?", new List<(string, Action)>()
                            {
                                ("Ja", async () => { await _customerRepository.DeleteCustomer(_customerVM.Customer.ID, () => {
                                    this.NavigationController.PopViewController(true);
                                });
                                }),
                                ("Nej", () => {  })
                            });
                        })
                    }, this);
                }); */

                //this.RightNavigationButton("Rediger", () => this.Start<AddCustomerVC>().Set_EditCustomer(_customerVM.Customer));
            }
		}

        void inviteCustomer() 
        {
            this.DisplayAlert("Inviter kunde", "Du er ved og sende en invitation til alle brugere knyttet til kunden. Vil du fortsætte?", new List<(string, Action)>
            {
                ("Ja", () => _customerRepository.Invite(_customerVM.Customer.ID, success: () => {
                            this.DisplayToast("Kunden er inviteret");
                        }).LoadingOverlay(this)),
                ("Nej", () => {})
            });
        }

        void DeleteCustomerOption(List<(string, Action)> options)
        {
            if (_customerVM.Customer == null)
                return;

            if (_customerVM.Customer.Disabled)
            {
                options.Add(("Aktiver kunde", () => {
                    this.DisplayAlert("Vil du genaktiverer denne kunde?", "Du er ved og aktiverer denne slettede kunde: " + _customerVM.Customer.Name + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                        ("Ja", () => EnableCustomer()),
                        ("Nej", () => {}),
                    });
                }
                ));
            }
            else
            {
                options.Add(("Slet kunde", () => {
                    this.DisplayAlert("Vil du slette denne kunde?", "Du er ved og slette kunde: " + _customerVM.Customer.Name + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                        ("Ja", () => DeleteCustomer()),
                        ("Nej", () => {}),
                });
                }
                ));
            }
        }

        void DeleteCustomer()
        {
            _customerRepository.Delete(_customerVM.Customer.ID, () =>
            {
                _customerVM.Customer.Disabled = true;
                this.DisplayToast("Kunde slettet");
            }).LoadingOverlay(this);
        }

        void EnableCustomer()
        {
            _customerRepository.Enable(_customerVM.Customer.ID, () =>
            {
                _customerVM.Customer.Disabled = false;
                this.DisplayToast("Kunde gendannet");
            }).LoadingOverlay(this);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            SetDetails();
        }

        async void SetDetails()
        {
            ContactLabel.Text = "";
            var customer = _customerVM.Customer;

            await _customerRepository.Get(customer.ID, c => customer = c);
            _customerVM.Customer = customer;

            TitleLabel.Text = customer.Name;

            if(_userVM.LoggedInUser().CustomerID != null) {
                CommentStack.Hidden = true;

                if (customer.KeyAccountManager != null)
                {
                    ContactLabel.Text = customer.KeyAccountManager.FirstName + " " + customer.KeyAccountManager.LastName + "\n" + customer.KeyAccountManager.Email + "\n" + customer.KeyAccountManager.Comment;
                }
            } else {
				CommentLabel.Text = customer.Comment;

                if (customer.MainUser != null)
                {
                    ContactLabel.Text = customer.MainUser.FirstName + " " + customer.MainUser.LastName + "\n" + customer.MainUser.Email + "\n" + customer.MainUser.Comment;
                }
            }

            /*
            if (!string.IsNullOrEmpty(customer.ImageLocation))
            {
                var imageArray = await _storage.DownloadImageArray(customer.ImageLocation);

                ImageView.UserInteractionEnabled = true;
                ImageView.GestureRecognizers = null;
                ImageView.AddGestureRecognizer(new UITapGestureRecognizer((obj) => {
                    this.Start<ImageVC>().ParseInfo(imageArray);
                }));

                ImageView.Image = new UIImage(NSData.FromArray(imageArray.ResizeImage(200)));
            }
*/

        }
    }
}