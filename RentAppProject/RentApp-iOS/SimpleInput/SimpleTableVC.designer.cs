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
    [Register ("SimpleTableVC")]
    partial class SimpleTableVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView Table { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitelLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Table != null) {
                Table.Dispose ();
                Table = null;
            }

            if (TitelLabel != null) {
                TitelLabel.Dispose ();
                TitelLabel = null;
            }
        }
    }
}