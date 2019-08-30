using System;
using UIKit;

namespace ModuleLibraryiOS.Search
{
    public static class Search
    {
        public static void AddSearch(this UIViewController vc, Action<string> SearchButtonClicked, UIColor color, Action<string> TextChanged = null)
        {
            var searchController = new UISearchController(searchResultsController: null)
            {
                HidesNavigationBarDuringPresentation = false,
                DimsBackgroundDuringPresentation = false,
            };
            searchController.SearchBar.SearchBarStyle = UISearchBarStyle.Minimal;
            searchController.SearchBar.TintColor = color;
            searchController.SearchBar.BarTintColor = color;
            if(TextChanged != null)
                searchController.SearchBar.TextChanged += (sender, e) => TextChanged.Invoke(searchController.SearchBar.Text);
            searchController.SearchBar.SearchButtonClicked += (sender, e) => SearchButtonClicked.Invoke(searchController.SearchBar.Text);
            vc.NavigationItem.SearchController = searchController;
        }
    }
}
