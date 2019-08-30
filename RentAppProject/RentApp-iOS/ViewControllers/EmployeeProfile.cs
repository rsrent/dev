using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.ViewControllerInstanciater;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Alert;
using System.Collections.Generic;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Image;
using RentApp.Repository;
using RentApp.ViewModels;
using Rent.Shared.ViewModels;
using System.Linq;

namespace RentApp
{
    public partial class EmployeeProfile : UIViewController
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        CustomerRepository customerRepository = AppDelegate.ServiceProvider.GetService<CustomerRepository>();
        UserRepository userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        LoginRepository _loginRepository = AppDelegate.ServiceProvider.GetService<LoginRepository>();
        LocationRepository locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
        public EmployeeProfile (IntPtr handle) : base (handle) { }

        User User;
        public void ParseInfo(User user) {
            User = user;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
			ImageView.Round();
            //SetInfo();

            var options = new List<(string, Action)>();

            options.Add(("Brugers lokaliteter", () => this.Start<LocationTableVC>().Set_GetUsersLocations(User) ));
            options.Add(("Brugers mulige lokaliteter", () => this.Start<LocationTableVC>().Set_GetNotUsersLocations(User) ));

            if (_userVM.HasPermission("User", Permission.CRUDD.Update) || (User != null && User.ID == _userVM.LoggedInUser().ID))
                options.Add(("Brugerdetaljer", () => {
                    this.Start<UserCreateVC>().SetEditUser(User);
            })); //this.Start<AddUserVC>().Set_EditUser(User)

            if (_userVM.HasPermission("Permission", Permission.CRUDD.Update))
                options.Add(("Brugerrettigheder", () => this.Start<UserPermissionsTableVC>().ParseInfo(User)));

            if ((User != null && User.ID == _userVM.LoggedInUser().ID))
                options.Add(("Opdater kodeord", () => this.Start<UpdatePasswordVC>()));

            if (_userVM.HasPermission("Login", Permission.CRUDD.Read))
            {
                options.Add(("Opdater kodeord", () => {
                    _loginRepository.GetUserUsername(User.ID, (username) =>
                    {
                        this.Start<UpdatePasswordVC>().ParseInfo(true, username);
                    }).LoadingOverlay(this);
                }));
            }

            if (_userVM.HasPermission("User", Permission.CRUDD.Delete))
                DeleteUserOption(options);
            
            if (options.Count > 0)
                this.RightNavigationButton("Rediger", (button) => this.DisplayMenu("Bruger muligheder", options, button));
        }

        void DeleteUserOption(List<(string, Action)> options)
        {
            if (User == null)
                return;

            if(User.Disabled)
            {
                options.Add(("Aktiver bruger", () => {
                    Alert.DisplayAlert(this, "Vil du genaktiverer denne bruger?", "Du er ved og aktiverer denne slettede brugeren " + User.FirstName + " " + User.LastName + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                        ("Ja", () => EnableUser()),
                        ("Nej", () => {}),
                    });
                }
                ));
            }
            else {
                options.Add(("Slet bruger", () => {
                    Alert.DisplayAlert(this, "Vil du slette denne bruger?", "Du er ved og slette brugeren " + User.FirstName + " " + User.LastName + ". Er du sikker på du vil fortsætte?", new List<(string, Action)> {
                    ("Ja", () => DeleteUser()),
                    ("Nej", () => {}),
                });
                }
                ));
            }
        }

        void DeleteUser() {
            userRepository.Disable(User, () =>
            {
                User.Disabled = true;
                this.DisplayToast("User deleted");
            }).LoadingOverlay(this);
        }

        void EnableUser()
        {
            userRepository.Enable(User, () =>
            {
                User.Disabled = false;
                this.DisplayToast("User enabled");
            }).LoadingOverlay(this);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            SetInfo();
        }

        async void SetInfo() {
            if (User != null)
            {
                NameLabel.Text = User.FirstName + " " + User.LastName;

                Title = User.FirstName;

                if (User.CustomerName != null) {
                    TitleLabel.Text = User.Title + ", " + User.CustomerName;
                }
                else
                    TitleLabel.Text = User.Title;

                DescriptionLabel.Text = User.Comment;

                if (!string.IsNullOrEmpty(User.ImageLocation))
                {
                    var imageArray = await _storage.DownloadImageArray(User.ImageLocation);
                    if (imageArray == null)
                        return;
                    ImageView.UserInteractionEnabled = true;
                    ImageView.GestureRecognizers = null;
                    ImageView.AddGestureRecognizer(new UITapGestureRecognizer((obj) => {
                        this.Start<ImageVC>().ParseInfo(imageArray);
                    }));

                    ImageView.Image = new UIImage(NSData.FromArray(imageArray.ResizeImage(200)));
                }

                if (_userVM.HasPermission("Login", Permission.CRUDD.Read))
                {
                    _loginRepository.GetUserUsername(User.ID, (username) =>
                    {
                        NameLabel.Text += "\n(" + username + ")";
                    });
                }
            } else {
                Title = "Bruger";
            }



            this.AddNavigationStack();
        }


        /*

        void GetUsersLocations () {

            var vc = this.Start<SimpleTableVC>();
            List<Location> loadedLocations = null;
            vc.Setup("Brugers lokaliteter", (Action<ICollection<(string, string, string)>> update) =>
            {
                locationRepository.GetUsersLocations(User, (locations) =>
                {
                    loadedLocations = locations;
                    update(locations.Select(l =>
                                           (l.Name,
                                            "",
                                            "")).ToList());
                }).LoadingOverlay(vc);
            }, (i) => {
                
            });
        }



        void GetNotUsersLocations()
        {
            var vc = this.Start<SimpleTableVC>();
            List<Location> loadedLocations = null;
            vc.Setup("Andre lokaliteter", (Action<ICollection<(string, string, string)>> update) =>
            {
                locationRepository.GetNotUsersLocations(User, (locations) =>
                {
                    loadedLocations = locations;
                    update(locations.Select(l =>
                                           (l.Name,
                                            "",
                                            "")).ToList());
                }).LoadingOverlay(vc);
            }, (i) => {

            });
        }

*/

    }
}