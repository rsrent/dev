// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ModuleLibraryiOS.Search
{
    [Register ("SearchViewController")]
    partial class SearchViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView AddedView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint AddedViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchBar SearchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchDisplayController searchDisplayController { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TableContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddedView != null) {
                AddedView.Dispose ();
                AddedView = null;
            }

            if (AddedViewHeightConstraint != null) {
                AddedViewHeightConstraint.Dispose ();
                AddedViewHeightConstraint = null;
            }

            if (SearchBar != null) {
                SearchBar.Dispose ();
                SearchBar = null;
            }

            if (searchDisplayController != null) {
                searchDisplayController.Dispose ();
                searchDisplayController = null;
            }

            if (TableContainer != null) {
                TableContainer.Dispose ();
                TableContainer = null;
            }
        }
    }
}