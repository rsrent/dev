using Foundation;
using System;
using UIKit;
using ModuleLibraryiOS.Table;
using System.Collections.Generic;
using System.Threading.Tasks;
using ModuleLibraryiOS.Alert;
using RentAppProject;
using RentApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.ViewControllerInstanciater;
using ModuleLibraryiOS.Navigation;
using CoreGraphics;
using System.Linq;
using static RentApp.AddLocationVC;
using Rent.Shared.ViewModels;

namespace RentApp
{
    public partial class EmployeeTableVC : ITableAndSourceViewController<User>
    {
        UserVM _userVM = AppDelegate.ServiceProvider.GetService<UserVM>();

        UserRepository _userRepository = AppDelegate.ServiceProvider.GetService<UserRepository>();
        LocationRepository _locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
        EmployeeTableVM _employeeVM = AppDelegate.ServiceProvider.GetService<EmployeeTableVM>();
        CustomerVM _customerVM = AppDelegate.ServiceProvider.GetService<CustomerVM>();
        //CreateUserVM _createUserVM = AppDelegate.ServiceProvider.GetService<CreateUserVM>();

        public EmployeeTableVC (IntPtr handle) : base (handle) { }

        Func<Action<ICollection<User>>, Task> LoadFunction;
        Action<User, NSIndexPath> ClickAction;
        UISegmentedControl segmentControl = new UISegmentedControl();
        bool hideDeleted = true;

        TableAndSourceController<EmployeeTableVC, User> TableAndSourceController;

        List<User> LoadedUsers = new List<User>();

        string searchText = "";

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
            TableAndSourceController = TableAndSourceController<EmployeeTableVC, User>.Start(this);

			segmentControl.Frame = new CGRect(20, 20, 200, 30);

			segmentControl.InsertSegment("Personale", 0, false);
            segmentControl.InsertSegment("Kunder", 1, false);			    
			segmentControl.SelectedSegment = 0;

			segmentControl.ValueChanged += (sender, e) => {
				var selectedSegmentId = (sender as UISegmentedControl).SelectedSegment;
                TableAndSourceController.ReloadTable();
			};
			this.NavigationItem.TitleView = segmentControl;

            SearchBar.TextChanged += (sender, e) => {
                searchText = SearchBar.Text;
                TableAndSourceController.ReloadTable(MatchingSearch());
            };

            SearchBar.SearchButtonClicked += (sender, e) => {
                SearchBar.ResignFirstResponder();
            };

            /*
            SearchBar.TextChanged += (sender, e) => {
                ReloadTable.Invoke(true);
            };
            SearchBar.SearchButtonClicked += (sender, e) => SearchBar.ResignFirstResponder();
            SearchBar.Hidden = true;
*/

            //var test = this.Start<EmployeeProfile>();
            //test.ParseInfo(_userVM.LoggedInUser());

            //this.NavigationController.NavigationBar.Add(new UIView(new CGRect(0,60,View.Bounds.Width, 50)));
            //this.NavigationController.NavigationBar.Bounds += 60;


            //var newView = Storyboard.InstantiateViewController("EmployeeProfile") as EmployeeProfile;
            /*
            var searchController = new UISearchController(searchResultsController: null)
            //var searchController = new UISearchController(searchResultsController: newView)
            {
                HidesNavigationBarDuringPresentation = false,
                DimsBackgroundDuringPresentation = true,
                //ObscuresBackgroundDuringPresentation = true



                //HidesNavigationBarDuringPresentation = false,
                //DimsBackgroundDuringPresentation = false,
                //ObscuresBackgroundDuringPresentation = false
            };
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
            searchController.SearchBar.TintColor = UIColor.FromName("ThemeColor");
            searchController.SearchBar.BarTintColor = UIColor.FromName("ThemeColor"); */

            /*
            searchController.SearchBar.TextChanged += (sender, e) =>
            {
                searchText = searchController.SearchBar.Text;
                if(searchText == "") {
                    ReloadTable.Invoke();
                }
            };
            searchController.SearchBar.SearchButtonClicked += (sender, e) => {
                ReloadTable.Invoke();
            };
            searchController.SearchBar.CancelButtonClicked += (sender, e) => {
                searchText = "";
                ReloadTable.Invoke();
            }; */

            //this.NavigationItem.SearchController = searchController;
            //this.NavigationItem.HidesSearchBarWhenScrolling = false; 

            //searchController.SearchResultsUpdater = this;
		}

        List<User> MatchingSearch()
        {
            //return LoadedUsers.Where(u => ((u.FirstName + " " + u.LastName + " " + u.Role.Name + (u.Customer != null && u.Customer.Name != null ? u.Customer.Name : ""))
            //                               .ToLower().Contains(searchText.ToLower())) || (new[] {"disabled", "slettet", "slettede", "deleted", "slet", "disable"}.Contains(searchText.ToLower()) && u.Disabled )).ToList();

            return LoadedUsers.Where(u => ((u.FirstName + " " + u.LastName + " " + u.RoleName + " " + u.CustomerName)
                                           .ToLower().Contains(searchText.ToLower())) || (new[] { "disabled", "slettet", "slettede", "deleted", "slet", "disable" }.Contains(searchText.ToLower()) && u.Disabled)).ToList();
        }

		public override UITableViewCell GetCell(NSIndexPath path, User val) 
            => TableView.StartCell<EmployeeCell>((cell) => { cell.UpdateCell(val); });

        public override UITableView GetTable() 
            => TableView;

        public override async Task RequestTableData(Action<ICollection<User>> updateAction)
        {
            await LoadFunction.Invoke((users) => {
                LoadedUsers = users.ToList();
                updateAction.Invoke(MatchingSearch());
            });
        }

        public override void RowSelected(NSIndexPath path, User val)
        {
            if(ClickAction != null)
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

        ICollection<User> Filter(ICollection<User> users) {
            /*
            users = users.Where(u => (u.FirstName + " " + u.LastName + " " + u.Role.Name)
                                .ToLower().Contains(searchText.ToLower()) 
                                && !AddedList.Any(a => a.ID == u.ID)
                                && (u.CustomerID == null 
                                    && segmentControl.SelectedSegment == 0 
                                    || u.CustomerID != null && segmentControl.SelectedSegment == 1)).ToList();
                                    */

            users = users.Where(u => !AddedList.Any(a => a.ID == u.ID))
                         .Where(u => segmentControl.SelectedSegment == 2 && u.Disabled ||
                               !u.Disabled && (u.CustomerID == null && segmentControl.SelectedSegment == 0
                                               || u.CustomerID != null && segmentControl.SelectedSegment == 1)).ToList();

            return users;
        }

        public EmployeeTableVC SetLoadSource_AllUsers()
		{
            segmentControl.InsertSegment("Slettede", 2, false);
			LoadFunction = async (updateAction) =>
			{
                await _userRepository.GetUsers((obj) => updateAction.Invoke(Filter(obj)));
			};

            if (_userVM.HasPermission(Permission.USER, Permission.CRUDD.Create))
            {
                this.RightNavigationButton("Ny", () => {

                    this.Start<UserCreateVC>().NewUser();

                    /*
                    this.Start<AddUserVC>().Set_NewUser(
                        "Ny bruger", 
                        (User user) => { }, 
                        async (User user) =>
                    {
                        await Task.Delay(1);
                        this.NavigationController.PopToViewController(this, true);
                    }); */

                });
            }

            ClickAction = (user, path) =>
            {
                this.Start<EmployeeProfile>().ParseInfo(user);
            };
            return this;
		}

        public void SetLoadSource_LocationUsers(Location location, Customer customer) {
            LoadFunction = async (updateAction) =>
            {
                await _userRepository.GetLocationUsers(location, (obj) => updateAction.Invoke(Filter(obj)));
            };

            if (_userVM.HasPermission(Permission.LOCATION, Permission.CRUDD.Update))
            {
                this.RightNavigationButton("Tilføj", () => {
                    this.Start<EmployeeTableVC>().SetLoadSource_PotentialLocationUsers(location, customer);
                });
            }

            ClickAction = (user, path) =>
            {
                var actions = new List<(string, Action)>();
                if(_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Read)) {
                    actions.Add(("Se profil", () => MenuItem_ViewUser(user)));
                }

                if (_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Update))
                {
                    actions.Add(("Opdater brugers titel", () => MenuItem_UpdateTitle(location, user)));
                }

                if (_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Update))
                {
                    actions.Add(("Opdater brugers timer tekst", () => MenuItem_UpdateHourText(location, user)));
                }

                if(_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Update)) {
                    actions.Add(("Fjern bruger fra lokalitet", () => MenuItem_RemoveUserFromLocation(location, user)));
                }

                if(actions.Count == 1) {
                    actions[0].Item2.Invoke();
                }
                else if(actions.Count() > 0)
                    this.DisplayMenu("Bruger muligheder", actions, source: TableView.CellAt(path));
            };
        }

        public void SetLoadSource_PotentialLocationUsers(Location location, Customer customer) {
			LoadFunction = async (updateAction) =>
			{
                await _userRepository.GetPotentialLocationUsers(location, (obj) => updateAction.Invoke(Filter(obj)));
			};
            this.RightNavigationButton("Ny",() => {
                
                int? customerID = null;
                if (segmentControl.SelectedSegment == 1)
                    customerID = customer.ID;

                this.Start<UserCreateVC>().NewUser(
                    customerID: customerID, completed: async (user) => {
                    await _locationRepository.AddUser(location.ID, user.ID, () =>
                    {
                        this.NavigationController.PopToViewController(this, false);
                        this.NavigationController.PopViewController(true);
                    }, () => this.DisplayToast("Bruger oprettet, men blev ikke tilføjet til lokation"));
                });

                /*
                this.Start<AddUserVC>().Set_NewUser(
                        "Ny bruger",
                    (User user) => { if (segmentControl.SelectedSegment == 1) user.CustomerID = customer.ID;  },
                        async (User user) =>
                        {
                            await _locationRepository.AddUser(location.ID, user.ID, "", () =>
                            {
                                this.NavigationController.PopToViewController(this, false);
                                this.NavigationController.PopViewController(true);
                            }, () => this.DisplayToast("Bruger oprettet, men blev ikke tilføjet til lokation"));
                        }); */
            });

            ClickAction = (user, path) =>
            {
                var actions = new List<(string, Action)>();
                if (_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Read)) {
                    actions.Add(("Se profil", () => MenuItem_ViewUser(user) ));
                }
                if (_userVM.HasPermission(Permission.TEAM, Permission.CRUDD.Update)) {
                    actions.Add(("Tilføj bruger til lokalitet", () => MenuItem_AddUserToLocation(location, user)));
                }

                if (actions.Count() > 0)
                    this.DisplayMenu("Bruger muligheder", actions, source: TableView.CellAt(path));
            };
        }

        public void SetLoadSource_LocationResponsible(Action<User> completeAction, bool serviceLeader, Customer customer) {
            segmentControl.Hidden = true;

            if(serviceLeader) {
                LoadFunction = async (updateAction) =>
                {
                    await _userRepository.GetUsersWithRole(Role.ServiceLeader.ID, (obj) => updateAction.Invoke(obj.Where(u => u.CustomerID == null).ToList()));
                };
                /*
                LoadFunction = async (updateAction) =>
                {
                    await _userRepository.GetUsers((obj) => updateAction.Invoke(obj.Where(u => u.CustomerID == null).ToList()));
                }; */
            } else {
                LoadFunction = async (updateAction) =>
                {
                    await _userRepository.GetCustomerUsers(customer, (obj) => updateAction.Invoke(obj.Where(u => u.CustomerID == null && serviceLeader || u.CustomerID != null && !serviceLeader).ToList()));
                };
            }

            this.RightNavigationButton("Ny", () => {

                int? customerID = customer.ID;
                int roleID = Role.LocationManager.ID;

                if (serviceLeader)
                {
                    customerID = null;
                    roleID = Role.ServiceLeader.ID;
                }

                this.Start<UserCreateVC>().NewUser(customerID, roleID, (user) =>
                {
                    completeAction.Invoke(user);
                });

                /*
                this.Start<AddUserVC>().Set_NewUser(
                    "Ny " + (serviceLeader ? Role.ServiceLeader : Role.LocationManager),
                    (User user) => 
                {
                    if (!serviceLeader)
                        user.CustomerID = customer.ID;
                },
                    async (User user) =>
                    {
                        await Task.Delay(1);
                        completeAction.Invoke(user);
                        this.NavigationController.PopToViewController(this, false);
                        this.NavigationController.PopViewController(true);
                }, serviceLeader ? Role.ServiceLeader : Role.LocationManager); */

            });

            ClickAction = (u, path) =>
            {
                completeAction.Invoke(u);
                this.NavigationController.PopViewController(true);
            };
        }

        public void SetLoading_UsersWithRole(Role role, Action<User> completeAction)
        {
            segmentControl.Hidden = true;
            LoadFunction = async (updateAction) =>
                await _userRepository.GetUsersWithRole(role.ID, (obj) => updateAction.Invoke(obj.ToList()));

            ClickAction = (u, path) =>
            {
                completeAction.Invoke(u);
                this.NavigationController.PopViewController(true);
            };
        }

        public void SetLoading_ConversationUsers(Conversation conversation)
        {
            segmentControl.Hidden = true;

            LoadFunction = async (updateAction) =>
            {
                updateAction.Invoke(conversation.Users);
            };

            /*
            this.RightNavigationButton("Tilføj", () => {
                this.Start<EmployeeTableVC>().SetLoading_PotentialConversationUsers(conversation);
            }); */

            ClickAction = (u, path) =>
            {
                /*
                Alert.DisplayMenu("", new List<(string, Action)> {
                    ("Tilføj bruger til samtale", () => {
                        AppDelegate.ServiceProvider.GetService<ChatRepository>().AddUser(conversation.ID, u.ID,() => {
                            this.DisplayToast("Bruger tilføjet til samtale");
                        });
                    })
                }, this);
                */

            };


        }

        public void SetLoading_PotentialConversationUsers(Conversation conversation, Action<UIViewController,ICollection<User>> selectedUsers) 
        {
            

            Func<ICollection<User>, ICollection<User>> conversationUsers = (user) =>
            {
                user = user.Where(u => u.ID != _userVM.ID).ToList();
                if (conversation == null)
                    return Filter(user);
                else
                    return Filter(user.Where(u => !conversation.Users.Any(cu => cu.ID == u.ID)).ToList());
            };

            LoadFunction = async (updateAction) =>
            {
                await _userRepository.GetUsers((obj) => {
                    LoadedUsers = obj.ToList();
                    updateAction.Invoke(conversationUsers(LoadedUsers));
                });
            };

            ClickAction = (u, path) =>
            {
                if(!AddedList.Any(a => a.ID == u.ID))
                    AddedList.Add(u);
                UpdateButtons(() => {
                    TableAndSourceController.ReloadTable(conversationUsers(LoadedUsers).ToList());
                });
                TableAndSourceController.ReloadTable(conversationUsers(LoadedUsers).ToList());
                /*
                this.DisplayMenu("", new List<(string, Action)> {
                    ("Tilføj bruger til samtale", () => {
                        AppDelegate.ServiceProvider.GetService<ChatRepository>().AddUser(conversation.ID, u.ID,() => {
                            this.DisplayToast("Bruger tilføjet til samtale");
                        }).LoadingOverlay(this, "Tilføjer bruger");
                    })
                }, source: TableView.CellAt(path)); */
            };

            this.RightNavigationButton("Start", () => {
                selectedUsers(this, AddedList);
            });
        }

        void MenuItem_AddUserToLocation(Location location, User user) {
            _locationRepository.AddUser(location.ID, user.ID, () =>
            {
                Alert.DisplayToast("Bruger tilføjet til lokalitet", this);
                TableAndSourceController.ReloadTable();
            }).LoadingOverlay(this, "Tilføjer bruger");
        }

        void MenuItem_RemoveUserFromLocation(Location location, User user)
		{
            _locationRepository.RemoveUser(location.ID, user.ID, () =>
            {
                Alert.DisplayToast("Bruger fjernet fra lokalitet", this);
                TableAndSourceController.ReloadTable();
            }).LoadingOverlay(this, "Fjerner bruger");
		}

        void MenuItem_UpdateTitle(Location location, User user)
        {
            this.DisplayTextField("Brugers titel", "Afløser...", (text) =>
            {
                _locationRepository.UpdateLocationUserTitle(location.ID, user.ID, text, () =>
                {
                    Alert.DisplayToast("Titel opdateret", this);
                }).LoadingOverlay(this);
            });
        }

        void MenuItem_UpdateHourText(Location location, User user)
        {
            this.DisplayTextField("Brugers time tekst", "arbejder 2 timer man-fre...", (text) =>
            {
                _locationRepository.UpdateLocationUserHourtext(location.ID, user.ID, text, () =>
                {
                    Alert.DisplayToast("Time tekst opdateret", this);
                }).LoadingOverlay(this);
            });
        }

		void MenuItem_ViewUser(User user)
		{
            this.Start<EmployeeProfile>().ParseInfo(user);
		}
















        List<UIButton> AddedButtons = new List<UIButton>();
        List<User> AddedList = new List<User>();

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

            foreach (User p in AddedList)
            {
                User user = p;
                string s = user.FirstName + "   -";

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
                    AddedList.Remove(user);
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

        /*
        public void UpdateSearchResultsForSearchController(UISearchController searchController)
        {
            //throw new NotImplementedException();

            searchText = searchController.SearchBar.Text;
            TableAndSourceController.RefreshTable(LoadedUsers.Where(u => (u.FirstName + " " + u.LastName + " " + u.Role.Name)
                                                                    .ToLower().Contains(searchText.ToLower())).ToList());
        } */
    }
}