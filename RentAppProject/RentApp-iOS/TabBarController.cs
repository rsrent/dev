using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using RentAppProject;
using RentApp.ViewModels;
using ModuleLibraryiOS.Alert;
using System.Threading.Tasks;
using ModuleLibraryiOS.Services;
using ModuleLibraryiOS.Storage;
using System.IO;
using System.Net;
using ModuleLibraryiOS.Web;
using RentApp.Repository;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class TabBarController : UITabBarController
    {
        UIColor TintColor = UIColor.FromName("ThemeColor");
        Settings _settings = AppDelegate.ServiceProvider.GetService<Settings>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        CustomerRepository customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        LocationRepository locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();

        CustomerVM customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        LocationVM locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();

        public TabBarController (IntPtr handle) : base (handle)
        {
        }  

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            CreateTabs();
            //this.TabBar.TintColor = TintColor;
            /*
            if (!_settings.Live)
            {
                this.TabBar.BackgroundColor = UIColor.FromRGB(70,230,255);
            }
            else
            {
                this.TabBar.TintColor = TintColor;
            }
*/
            //Test();



            //TODO WILL FIX IMAGEs
            //ImageFixer.TryFixAll();
        }

        async void CreateTabs() {
            var rootUser = _userVM.LoggedInUser();
            //var userTitle = rootUser.Role.Name;

            var viewList = new List<UIViewController>();








            /*
            if (rootUser.Role.ID == 1) 
            {
                viewList.Add(GetCustomerTable());
                viewList.Add(GetUserTable());
            }
            if (rootUser.Role.ID == 2)
            {
                viewList.Add(GetCustomerTable());
                viewList.Add(GetUserTable());
            }
            if (rootUser.Role.ID == 3)
            {
                //TODO - fix back
                viewList.Add(GetCustomerTable());
                viewList.Add(GetUserTable());
                //viewList.Add(GetLocationTable());
            }
            if (rootUser.Role.ID == 4)
            {
                
            }
            if (rootUser.Role.ID == 5 || rootUser.Role.ID == 6 || rootUser.Role.ID == 7)
            {
                viewList.Add(GetLocationTable());
            }
            */
            if (rootUser.RoleID == 9)
            {
                await customerRepository.GetForCustomerUser(rootUser, (customer) =>
                {
                    customerVM.Customer = customer;
                    viewList.Add(GetCustomerMenu());
                });
            }
            else if (rootUser.RoleID == 8)
            {
                var customer = await customerRepository.GetForCustomerUser(rootUser, null);
                if(customer != null) {
                    var locations = await locationRepository.GetForUser(rootUser, null);
                    if (locations == null || locations.Count == 0)
                    {
                        Alert.DisplayToast("Login fejl", this);
                    }
                    else if (locations.Count == 1)
                    {
                        //base.NavigationController.PopViewController(true);
                        locationVM.Location = locations[0];
                        viewList.Add(GetLocationMenu());
                    }
                    else
                    {
                        viewList.Add(GetLocationTable());
                    }
                }
            }
            else {
                if (_userVM.HasPermission("Customer", Permission.CRUDD.Read))
                {
                    viewList.Add(GetCustomerTable());
                }
                if (_userVM.HasPermission("Location", Permission.CRUDD.Read))
                {
                    viewList.Add(GetLocationTable());
                }

                if (_userVM.HasPermission("User", Permission.CRUDD.Read))
                {
                    viewList.Add(GetUserTable());
                }
            }

            //viewList.Add(GetChat());

            if (rootUser.RoleID == 1 || rootUser.RoleID == 2)
            {
                viewList.Add(GetSpecialFunctions());
            }

            viewList.Add(GetProfile());

            var navigationViewList = new List<UINavigationController>();
            foreach (var vc in viewList) {
                var newNC = new UINavigationController(vc);
                /*
                if(!_settings.Live) {
                    newNC.NavigationBar.BackgroundColor = UIColor.FromRGB(70, 230, 255);
                }
                else {
					newNC.NavigationBar.TintColor = TintColor;
                }*/
                //newNC.NavigationBar.TintColor = TintColor;
                navigationViewList.Add(newNC);
            }
            this.SetViewControllers(navigationViewList.ToArray(), true);

            UIView v = null;
            foreach (var vc in viewList)
                v = vc.View;
        }

        UIViewController GetProfile() 
        {
            var profileVC = this.Get<EmployeeProfile>();
            profileVC.ParseInfo(_userVM.LoggedInUser());
            profileVC.Title = "Profil";
            //profileVC.TabBarItem.Image
            profileVC.NavigationItem.LeftBarButtonItem = new UIBarButtonItem("Log ud", UIBarButtonItemStyle.Done, (sender, e) =>
            {
                this.DisplayMenu("Er du sikker på, at du vil logge ud?", new List<(string, Action)> {
                    ("Ja", async () => {   
                                            AppDelegate.ServiceProvider.GetService<LoginVM>().LogOut();
                                            this.NavigationController.PopToRootViewController(true);
                                            await Task.Delay(200);
                                            AppDelegate.ServiceProvider.GetService<MyConversationSocketConnection>().CloseConnection();
                                            LoginVC.Start(AppDelegate.ServiceProvider.GetService<RootViewModel>().Root);
                    }), ("Nej", () => {})
                }, profileVC.NavigationItem.LeftBarButtonItem);
            });
            profileVC.TabBarItem.Image = UIImage.FromBundle("profile");
            var v = profileVC.View;
            return profileVC;
        }

        UIViewController GetChat() {
            var chatVC = this.Get<ConversationsTableVC>();
            AppDelegate.ConversationTable = chatVC;
            chatVC.Title = "Beskeder";
            chatVC.TabBarItem.Image = UIImage.FromBundle("chat");
            return chatVC;
        }

        UIViewController GetCustomerTable()
        {
            var customersTableVC = this.Get<CustomersTableVC>();
            customersTableVC.Title = "Kunder";
            customersTableVC.TabBarItem.Image = UIImage.FromBundle("customer");
            return customersTableVC;
        }

        UIViewController GetLocationTable()
        {
            var locationTableVC = this.Get<LocationTableVC>();
            locationTableVC.Set_ViewForUser();
            locationTableVC.Title = "Lokationer";
            locationTableVC.TabBarItem.Image = UIImage.FromBundle("location");
            return locationTableVC;
        }

        UIViewController GetUserTable()
        {
            var employeeTableVC = this.Get<EmployeeTableVC>().SetLoadSource_AllUsers();
            employeeTableVC.Title = "Brugere";
            employeeTableVC.TabBarItem.Image = UIImage.FromBundle("users");
            return employeeTableVC;
        }

        UIViewController GetCustomerMenu()
        {
            var customersMenuVC = this.Get<CustomerMenuVC>();
            customersMenuVC.Title = "Hjem";
            customersMenuVC.TabBarItem.Image = UIImage.FromBundle("home");
            return customersMenuVC;
        }

        UIViewController GetLocationMenu()
        {
            var locationMenuVC = this.Get<LocationMenuVC>();
            locationMenuVC.Title = "Hjem";
            locationMenuVC.TabBarItem.Image = UIImage.FromBundle("home");
            return locationMenuVC;
        }

        UIViewController GetSpecialFunctions()
        {
            var specialFunctionsMenu = this.Get<SpecialFunctionsTableVC>();
            specialFunctionsMenu.Title = "Specialfunktioner";
            specialFunctionsMenu.TabBarItem.Image = UIImage.FromBundle("rent_icon");
            return specialFunctionsMenu;
        }
    }
}