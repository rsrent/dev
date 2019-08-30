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
    [Register ("AddressSearchVC")]
    partial class AddressSearchVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISearchBar SearchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView Tabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (SearchBar != null) {
                SearchBar.Dispose ();
                SearchBar = null;
            }

            if (Tabel != null) {
                Tabel.Dispose ();
                Tabel = null;
            }
        }
    }
}