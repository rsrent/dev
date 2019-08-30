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
    [Register ("UpdatePasswordVC")]
    partial class UpdatePasswordVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NewPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField OldPassword { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView OldPasswordStack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UserName { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (NewPassword != null) {
                NewPassword.Dispose ();
                NewPassword = null;
            }

            if (OldPassword != null) {
                OldPassword.Dispose ();
                OldPassword = null;
            }

            if (OldPasswordStack != null) {
                OldPasswordStack.Dispose ();
                OldPasswordStack = null;
            }

            if (UserName != null) {
                UserName.Dispose ();
                UserName = null;
            }
        }
    }
}