using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.ViewControllerInstanciater;
using RentAppProject;
using ModuleLibraryiOS.Alert;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.Storage;
using ModuleLibraryiOS.Camera;
using System.Threading.Tasks;
using ModuleLibraryiOS.Image;
using RentApp.Repository;
using System.Linq;
using Rent.Shared.ViewModels;
using ModuleLibraryiOS.Map;

namespace RentApp
{
    public partial class AddLocationVC : UIViewController
    {
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();
        LocationRepository _locationRepository;

        Customer Customer;
        AddressSearchVM _addressSearchVM;
        Location Location;
        NSData imageData;

        public AddLocationVC(IntPtr handle) : base(handle)
        {
            _locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
            _addressSearchVM = AppDelegate.ServiceProvider.GetService<AddressSearchVM>();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ImageView.Round();

            AddressTF.EditingDidBegin += (sender, e) =>
            {
                _addressSearchVM.ItemSeletedAction = async (address) =>
                {
                    var results = await Address.GetAddressPosition(address);

                    Location.Lat = results.Latitude;
                    Location.Lon = results.Longitude;
                    AddressTF.Text = address.forslagstekst;
                    NavigationController.PopToViewController(this, true);
                };
                _addressSearchVM.Address = AddressTF.Text;
                this.Start<AddressSearchVC>();
            };

            if(new int?[] { 1, 2 }.Contains(_userVM.LoggedInUser().RoleID)) {
                ServiceLeaderStack.Hidden = false;
                if (Location != null)
                {
                    IntervalOfServiceLeaderMeetingTF.Text = Location.IntervalOfServiceLeaderMeeting + "";
                }
            } else {
                ServiceLeaderStack.Hidden = true;
            }

            if(Location != null) {
                NameTF.Text = Location.Name;
                AddressTF.Text = Location.Address;
                CommentTV.Text = Location.Comment;
                PhoneTF.Text = Location.Phone;
                if(Location.ProjectNumber != null)
                    ProjectNumberTF.Text = Location.ProjectNumber + "";
            }

            ImageButton.TouchUpInside += (sender, e) => {
                this.Start<CameraContainerViewController>("Camera").EnableImage((data) =>
                {
                    imageData = data;
                    ImageView.Image = new UIImage(imageData);
                });
            };



            /*
            SelectServiceLeaderButton.TouchUpInside += (sender, e) => {
                if (updatedMainUser == null)
                    updatedMainUser = new UpdatedMainUser();
                this.Start<EmployeeTableVC>().SetLoadSource_LocationResponsible(updatedMainUser, true, Customer);
            };

            SelectCustomerContactButton.TouchUpInside += (sender, e) => {
                if (updatedMainUser == null)
                    updatedMainUser = new UpdatedMainUser();
                this.Start<EmployeeTableVC>().SetLoadSource_LocationResponsible(updatedMainUser, false, Customer);
            };
            */

            SelectServiceLeaderButton.Hidden = true;
            SelectCustomerContactButton.Hidden = true;

            SelectServiceLeaderButton.TouchUpInside += (sender, e) => {
                this.Start<EmployeeTableVC>().SetLoadSource_LocationResponsible(async (user) => {
                    Location.ServiceLeader = user;
                    await _locationRepository.UpdateLocation(Location,() => {});
                }, true, Customer);
            };

            SelectCustomerContactButton.TouchUpInside += (sender, e) => {
                this.Start<EmployeeTableVC>().SetLoadSource_LocationResponsible(async (user) => {
                    Location.CustomerContact = user;
                    await _locationRepository.UpdateLocation(Location, () => { });
                }, false, Customer);
            };
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            if(Location != null) {
                if(Location.ServiceLeader != null)
                    SelectServiceLeaderButton.SetTitle("Serviceleder: " + Location.ServiceLeader.FirstName, UIControlState.Normal);

                if (Location.CustomerContact != null)
                    SelectCustomerContactButton.SetTitle("Lokationsansvarlig: " + Location.CustomerContact.FirstName, UIControlState.Normal);
            }
            TabBarController.TabBar.Hidden = false;
            LoadImage();
        }

        async void LoadImage()
        {
            if (Location != null && !string.IsNullOrEmpty(Location.ImageLocation))
            {
                ImageView.Image = await _storage.DownloadImage(Location.ImageLocation, 200);
            }
        }

        public void Set_NewLocation(Customer customer) {
            Customer = customer;
            Location = new Location();
            Location.IntervalOfServiceLeaderMeeting = 30;
            this.RightNavigationButton("Tilføj", async () => 
            {
                var loc = await GetLocation();
                if (loc != null) 
                {
                    _locationRepository.AddLocation(loc, Customer, async (l) =>
                    {
                        Alert.DisplayToast("Lokalitet tilføjet", this);
                        await Task.Delay(500);
                        this.NavigationController.PopViewController(true);
                    }).LoadingOverlay(this);
                }
            });
        }

        void TryAddUserToLocation(Location location) {
            /*
            this.Start<AddUserVC>().Set_NewLocationUser(
                Customer.ID, 
                async (User user) => { 
                location.MainUser = user;
                await _locationRepository.UpdateLocation(location, success: UserAdded); 
            });
            */
        }

		void UserAdded()
		{
			NavigationController.PopToViewController(this, false);
			NavigationController.PopViewController(true);
		}

		public void Set_EditLocation(Customer customer, Location location)
		{
            Customer = customer;
            Location = location;
			this.RightNavigationButton("Opdater", async () => {

                //Location = await GetLocation();

                //UpdateMainUser(Location);
                var loc = await GetLocation();
                if(loc != null) {
                    _locationRepository.UpdateLocation(loc, async () =>
                    {
                        Alert.DisplayToast("Lokalitetsinformation opdateret", this);
                        await Task.Delay(500);
                        this.NavigationController.PopViewController(true);
                    }).LoadingOverlay(this);
                }
			});
		}

        async Task<Location> GetLocation()
        {
            var location = Location;
            location.Address = AddressTF.Text;
            location.Name = NameTF.Text;
            location.Comment = CommentTV.Text;
            location.Phone = PhoneTF.Text;

            if(int.TryParse(IntervalOfServiceLeaderMeetingTF.Text, out var interval)) {
                location.IntervalOfServiceLeaderMeeting = interval;
            } 

            location.ProjectNumber = null;
            if(int.TryParse(ProjectNumberTF.Text, out var projectNumber)) {
                location.ProjectNumber = projectNumber;
            } else if (!String.IsNullOrWhiteSpace(ProjectNumberTF.Text)) {
                this.DisplayToast("Projektnummer er ikke et tal");
                return null;
            } 

            if (string.IsNullOrEmpty(location.Name))
            {
                this.DisplayToast("Navn skal udfyldes");
                return null;
            }
            if (string.IsNullOrEmpty(location.Address))
            {
                this.DisplayToast("Adresse skal udfyldes");
                return null;
            }

            if (imageData != null)
            {
                var imageStream = imageData.AsStream();
                location.ImageLocation = await _storage.Upload(imageStream, "image", location.ImageLocation);
            }
            return location;
        }
    }
}