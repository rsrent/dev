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

namespace ModuleLibraryiOS.Map
{
    [Register ("MapViewController")]
    partial class MapViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        ModuleLibraryiOS.Map.MKMapViewController Map { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint MapHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchBar SearchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TableContainer { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Map != null) {
                Map.Dispose ();
                Map = null;
            }

            if (MapHeightConstraint != null) {
                MapHeightConstraint.Dispose ();
                MapHeightConstraint = null;
            }

            if (SearchBar != null) {
                SearchBar.Dispose ();
                SearchBar = null;
            }

            if (SelectButton != null) {
                SelectButton.Dispose ();
                SelectButton = null;
            }

            if (TableContainer != null) {
                TableContainer.Dispose ();
                TableContainer = null;
            }
        }
    }
}