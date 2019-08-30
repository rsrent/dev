using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Navigation;
using ModuleLibraryiOS.ViewControllerInstanciater;
using System.Linq;
using Rent.Shared.ViewModels;
using CoreGraphics;
using ModuleLibraryiOS.Alert;

namespace RentApp
{
    public partial class LocationTableVC : ITableAndSourceViewController<Location>
    {
        LocationRepository _locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
        LocationVM _locationVM = AppDelegate.ServiceProvider.GetService<LocationVM>();
        CustomerVM _customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();


        Action<Location, NSIndexPath> ClickAction;
        Func<Action<ICollection<Location>>, Task> LoadFunction;

        public LocationTableVC (IntPtr handle) : base (handle) { }

        List<Location> LoadedLocations = new List<Location>();
        bool showDeleted = false;

        string searchText = "";

        TableAndSourceController<LocationTableVC, Location> TableAndSourceController;

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableAndSourceController = TableAndSourceController<LocationTableVC, Location>.Start(this);
            Title = "Lokaliteter";
            this.AddNavigationStack();

            SearchBar.TextChanged += (sender, e) => {
                searchText = SearchBar.Text;
                TableAndSourceController.ReloadTable(MatchingSearch());
            };

            SearchBar.SearchButtonClicked += (sender, e) => {
                SearchBar.ResignFirstResponder();
            };
		}

        List<Location> MatchingSearch()
        {
            return LoadedLocations
                .Where(l => !AddedList.Any(a => a.ID == l.ID))
                .Where(l => ((l.Name + " " + l.CustomerName)
                             .ToLower().Contains(searchText.ToLower())))
                .Where(l => showDeleted && l.Disabled || !showDeleted && !l.Disabled).ToList();
        }

		public override UITableViewCell GetCell(NSIndexPath path, Location val)
        {
            return TableFunctions.InstanciateCell<LocationCell>(TableView, "LocationCell", (cell) => { cell.UpdateCell(val); });
		}

        public override UITableView GetTable()
        {
            return TableView;
        }

        public override async Task RequestTableData(Action<ICollection<Location>> updateAction)
        {
            if(LoadFunction != null) {
                await LoadFunction.Invoke((users) => {
                    LoadedLocations = users.ToList();
                    updateAction.Invoke(MatchingSearch());
                });
            }

            /*
            if(Customer == null) {
                await _locationRepository.GetForUser(_userVM.LoggedInUser(), (obj) => {
                    LoadedLocations = obj;
                    updateAction.Invoke(MatchingSearch());
                });
            } else {
                await _locationRepository.GetForCustomer(Customer, (obj) => {
                    LoadedLocations = obj;
                    updateAction.Invoke(MatchingSearch());
                });
            }
            */
        }

        public override void RowSelected(NSIndexPath path, Location val)
        {
            //if(val.Customer != null)_customerVM.Customer = val.Customer;
            //_locationVM.Location = val;

            if (ClickAction != null)
                ClickAction.Invoke(val, path);

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

        public void Set_ViewForUser() {

            var segmentControl = new UISegmentedControl();

            segmentControl.Frame = new CGRect(20, 20, 200, 30);

            segmentControl.InsertSegment("Mine", 0, false);

            var allowedIds = new int[] {1, 2, 3 };

            if(_userVM.LoggedInUser().CustomerID == null && allowedIds.Any(i => i == _userVM.LoggedInUser().RoleID))
            {
                segmentControl.InsertSegment("Alle", 1, false);
                segmentControl.InsertSegment("Slettede", 2, false);
            }

            segmentControl.SelectedSegment = 0;

            segmentControl.ValueChanged += (sender, e) => {
                var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
                TableAndSourceController.ReloadTable();
                showDeleted = selectedSegmentId == 2;
            };
            this.NavigationItem.TitleView = segmentControl;

            LoadFunction = async (updateAction) =>
            {
                if(segmentControl.SelectedSegment == 0){
                    await _locationRepository.GetForUser(_userVM.LoggedInUser(), (obj) => {
                        var ls = new List<Location>();
                        foreach (var l in obj)
                            if (!ls.Any(l1 => l1.ID == l.ID)) 
                                ls.Add(l);
                        updateAction.Invoke(ls);
                    });
                } else {
                    await _locationRepository.GetAll((obj) => {
                        updateAction.Invoke(obj);
                    });
                }

            };

            ClickAction = (location, path) =>
            {
                if (location.Customer != null) _customerVM.Customer = location.Customer;
                _locationVM.Location = location;
                this.Start<LocationMenuVC>();
            };
        }

		public void Set_ViewForCustomer(Customer customer)
		{
            LoadFunction = async (updateAction) =>
            {
                await _locationRepository.GetForCustomer(customer, (obj) => {
                    updateAction.Invoke(obj);
                });
            };

            ClickAction = (location, path) =>
            {
                if (location.Customer != null) _customerVM.Customer = location.Customer;
                _locationVM.Location = location;
                this.Start<LocationMenuVC>();
            };

            if (_userVM.HasPermission(Permission.LOCATION, Permission.CRUDD.Create))
            {
                this.RightNavigationButton("Ny", () => this.Start<AddLocationVC>().Set_NewLocation(_customerVM.Customer));
            }
		}

        public void Set_GetUsersLocations(User user)
        {
            LoadFunction = async (updateAction) =>
            {
                await _locationRepository.GetUsersLocations(user, (locations) =>
                {
                    updateAction(locations);
                });
            };

            ClickAction = (l, path) =>
            {
                if (!AddedList.Any(a => a.ID == l.ID))
                    AddedList.Add(l);
                UpdateButtons(() =>
                {
                    TableAndSourceController.ReloadTable(MatchingSearch());
                });
                TableAndSourceController.ReloadTable(MatchingSearch());
            };

            if (_userVM.HasPermission(Permission.LOCATION, Permission.CRUDD.Update))
            {
                this.RightNavigationButton("Fjern", () => {
                    this.DisplayAlert(
                        "Er du sikker?",
                        "Du er ved at fjerne brugeren fra følgende lokaliteter, er du sikker på at du vil fortsætte?",
                        new List<(string, Action)>
                    { ("Ja", () => RemoveUserFromLocations(user)), ("Nej", () => {})
                    });
                });
            }
        }

        void RemoveUserFromLocations(User user) {
            _locationRepository.RemoveUserFromLocations(user.ID, AddedList.Select(a => a.ID).ToList(), () =>
            {
                AddedList = new List<Location>();
                TableAndSourceController.ReloadTable();
                UpdateButtons(() =>
                {
                    TableAndSourceController.ReloadTable(MatchingSearch());
                });
            }).LoadingOverlay(this);
        }

        public void Set_GetNotUsersLocations(User user)
        {
            LoadFunction = async (updateAction) =>
            {
                await _locationRepository.GetNotUsersLocations(user, (locations) =>
                {
                    updateAction(locations);
                });
            };

            ClickAction = (l, path) =>
            {
                if (!AddedList.Any(a => a.ID == l.ID))
                    AddedList.Add(l);
                UpdateButtons(() =>
                {
                    TableAndSourceController.ReloadTable(MatchingSearch());
                });
                TableAndSourceController.ReloadTable(MatchingSearch());
            };

            if (_userVM.HasPermission(Permission.LOCATION, Permission.CRUDD.Update))
            {
                this.RightNavigationButton("Tilføj", () => {
                    this.DisplayAlert(
                        "Er du sikker?",
                        "Du er ved at tilføje brugeren til følgende lokaliteter, er du sikker på at du vil fortsætte?",
                        new List<(string, Action)>
                    { ("Ja", () => AddUserToLocations(user)), ("Nej", () => {})
                    });
                });
            }
        }

        void AddUserToLocations(User user)
        {
            _locationRepository.AddUserToLocations(user.ID, AddedList.Select(a => a.ID).ToList(), () =>
            {
                AddedList = new List<Location>();
                TableAndSourceController.ReloadTable();
                UpdateButtons(() =>
                {
                    TableAndSourceController.ReloadTable(MatchingSearch());
                });
            }).LoadingOverlay(this);
        }

        List<UIButton> AddedButtons = new List<UIButton>();
        List<Location> AddedList = new List<Location>();

        void UpdateButtons(Action RefreshAction)
        {

            float rowWidth = 10000000;
            float rowHeight = 40;
            float columnHeight = -rowHeight;
            AddedViewHeightConstraint.Constant = 0;

            foreach (UIButton b in AddedButtons)
            {
                b.RemoveFromSuperview();
            }
            AddedButtons.Clear();

            UIColor color = UIColor.FromName("ThemeColor");

            foreach (Location p in AddedList)
            {
                Location location = p;
                string s = location.Name + "   -";

                if (rowWidth + sizeOfString(s) + 6 > AddedView.Bounds.Width)
                {
                    columnHeight += rowHeight;
                    AddedViewHeightConstraint.Constant += rowHeight;
                    rowWidth = 0;
                }
                var button = new UIButton(new CGRect(rowWidth + 3, columnHeight + 3, sizeOfString(s), rowHeight - 6));

                button.SetTitle(s, UIControlState.Normal);
                button.SetTitleColor(UIColor.White, UIControlState.Normal);
                button.Layer.CornerRadius = 8.0f;
                button.BackgroundColor = color;
                AddedView.AddSubview(button);
                rowWidth += (float)button.Bounds.Width + 6;
                AddedButtons.Add(button);

                button.TouchUpInside += (sender, e) =>
                {
                    AddedList.Remove(location);
                    RefreshAction.Invoke();
                    UpdateButtons(RefreshAction);
                    //UpdateTable();
                };
            }

            float sizeOfString(string theString)
            {
                return (float)theString.StringSize(UIFont.SystemFontOfSize(17f)).Width + 20f;
            }
        }
    }
}