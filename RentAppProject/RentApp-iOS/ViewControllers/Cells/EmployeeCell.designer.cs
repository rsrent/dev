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
    [Register ("EmployeeCell")]
    partial class EmployeeCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel HourLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel RoleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HourLabel != null) {
                HourLabel.Dispose ();
                HourLabel = null;
            }

            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (RoleLabel != null) {
                RoleLabel.Dispose ();
                RoleLabel = null;
            }
        }
    }
}