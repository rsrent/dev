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

namespace RentApp
{
    [Register ("LocationTableVC")]
    partial class LocationTableVC
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
        UIKit.UITableView TableView { get; set; }

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

            if (TableView != null) {
                TableView.Dispose ();
                TableView = null;
            }
        }
    }
}