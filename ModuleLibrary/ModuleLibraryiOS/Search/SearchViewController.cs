using Foundation;
using System;
using UIKit;
using System.Collections.Generic;
using System.Threading.Tasks;
using CoreGraphics;

namespace ModuleLibraryiOS.Search
{
    public partial class SearchViewController : UIViewController
    {
        List<SearchType> searchedProfiles;
        List<SearchType> addedList = new List<SearchType>();
        List<UIButton> addedButtons = new List<UIButton>();
        Action<List<SearchType>> createAction;
        Func<string, Task<List<SearchType>>> getProfilesToInvite;
        Func<bool, Task> reloadTable;

        public SearchViewController(IntPtr handle) : base(handle)
        {
        }

        public static void StartNewChat(UIViewController viewController, UINavigationController navigationController, Func<string, Task<List<SearchType>>> profilesToInvite, Action<List<SearchType>> create) 
        {
            var chatStoryboard = UIStoryboard.FromName("Search", null);
            var newView = chatStoryboard.InstantiateViewController("SearchViewController") as SearchViewController;
            if (navigationController != null) navigationController.PushViewController(newView, true);
            else viewController.PresentViewController(newView, true, null);
            newView.parseInfo(profilesToInvite, create);
        }

        private void parseInfo(Func<string, Task<List<SearchType>>> profilesToInvite, Action<List<SearchType>> create)
        {
            this.getProfilesToInvite = profilesToInvite;
            this.createAction = create;
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var btn = new UIBarButtonItem("Create", UIBarButtonItemStyle.Plain, (sender, args) =>
            {
                createAction.Invoke(addedList);
            });
            this.NavigationItem.SetRightBarButtonItem(btn, true);

            SearchBar.TextChanged += (sender, e) =>
            {
                UpdateTable();
            };
            UpdateTable();
        }

        async void UpdateTable()
        {
            searchedProfiles = await getProfilesToInvite.Invoke(SearchBar.Text);

            /*
			reloadTable = Table.TableController.StartTable(TableContainer, this, (table, row, obj) => {
                var profile = obj as SearchType;
                return Table.TableController.InstanciateCell<SearchCell>(table, SearchCell.Key, (cell) => { cell.UpdateCell(profile, addedList.Find(s => s.GetUniqueId() == profile.GetUniqueId()) != null); });
			}, (row, obj) => { addToAddedList(searchedProfiles[row]);
			}, (obj) => { obj.RegisterNibForCellReuse(SearchCell.Nib, SearchCell.Key);
            }, searchedProfiles); */

            var source = new Table.TableSource<SearchType>((table, row, obj) =>
            {
                return Table.TableFunctions.InstanciateCell<SearchCell>(table, SearchCell.Key, (cell) => { cell.UpdateCell(obj, addedList.Find(s => s.GetUniqueId() == obj.GetUniqueId()) != null); });
			}, out var updateList, (row, obj) => {
				addToAddedList(searchedProfiles[row]);
			});
            updateList.Invoke(searchedProfiles);

            reloadTable = Table.TableViewController.Start(this, TableContainer).Initialize(source, (table, view) => { 
                table.RegisterNibForCellReuse(SearchCell.Nib, SearchCell.Key); 
            }).GetRefreshFunction();

            reloadTable.Invoke(true);
        }

        float sizeOfString(string theString)
        {
            return (float)theString.StringSize(UIFont.SystemFontOfSize(17f)).Width + 20f;
        }

        public void addToAddedList(SearchType item)
        {
            SearchBar.Text = "";
            if (addedList.Find(s => s.GetUniqueId() == item.GetUniqueId()) == null) addedList.Add(item);
            UpdateButtons();
            UpdateTable();
        }

        void UpdateButtons()
        {
            AddedViewHeightConstraint.Constant = 0;
            float rowWidth = 10000000;
            float columnHeight = -40;
            foreach (UIButton b in addedButtons)
            {
                b.RemoveFromSuperview();
            }
            addedButtons.Clear();

            UIColor color = new UIColor(0.7f, 0.8f, 0.9f, 1f);

            foreach (SearchType p in addedList)
            {
                SearchType profile = p;
                string s = profile.GetText() + "   -";

                if (rowWidth + sizeOfString(s) + 6 > AddedView.Bounds.Width)
                {
                    columnHeight += 40;
                    AddedViewHeightConstraint.Constant += 40;
                    rowWidth = 0;
                }
                var button = new UIButton(new CGRect(rowWidth + 3, columnHeight + 3, sizeOfString(s), 34));

                button.SetTitle(s, UIControlState.Normal);
                button.SetTitleColor(UIColor.White, UIControlState.Normal);
                button.Layer.CornerRadius = 8.0f;
                button.BackgroundColor = color;
                AddedView.AddSubview(button);
                rowWidth += (float)button.Bounds.Width + 6;
                addedButtons.Add(button);

                button.TouchUpInside += (sender, e) =>
                {
					addedList.Remove(profile);
					UpdateButtons();
					UpdateTable();
                };
            }
        }

        public static List<SearchType> getSearchHits(List<SearchType> options, string text) {
            List<SearchType> searchHits = new List<SearchType>();
            foreach(SearchType option in options) {
                if (option.GetText().ToLower().Contains(text.ToLower())) searchHits.Add(option);
            }
            return searchHits;
        }
    }
}