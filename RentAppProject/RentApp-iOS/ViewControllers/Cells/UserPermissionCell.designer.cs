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
    [Register ("UserPermissionCell")]
    partial class UserPermissionCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISwitch ActiveSwitch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CRUDDLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActiveSwitch != null) {
                ActiveSwitch.Dispose ();
                ActiveSwitch = null;
            }

            if (CRUDDLabel != null) {
                CRUDDLabel.Dispose ();
                CRUDDLabel = null;
            }
        }
    }
}