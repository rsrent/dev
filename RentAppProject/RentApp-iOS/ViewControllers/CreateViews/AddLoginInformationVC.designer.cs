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
    [Register ("AddLoginInformationVC")]
    partial class AddLoginInformationVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PasswordTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField UserNameTF { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (PasswordTF != null) {
                PasswordTF.Dispose ();
                PasswordTF = null;
            }

            if (UserNameTF != null) {
                UserNameTF.Dispose ();
                UserNameTF = null;
            }
        }
    }
}