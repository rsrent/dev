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
    [Register ("CustomerCreateVC")]
    partial class CustomerCreateVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Comment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CustomerManager { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel KeyAccountManager { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Name { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Comment != null) {
                Comment.Dispose ();
                Comment = null;
            }

            if (CustomerManager != null) {
                CustomerManager.Dispose ();
                CustomerManager = null;
            }

            if (KeyAccountManager != null) {
                KeyAccountManager.Dispose ();
                KeyAccountManager = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }
        }
    }
}