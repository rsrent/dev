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
    [Register ("SalesMainMenuViewController")]
    partial class SalesMainMenuViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ExistingCustomersButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton NewCustomerButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ExistingCustomersButton != null) {
                ExistingCustomersButton.Dispose ();
                ExistingCustomersButton = null;
            }

            if (NewCustomerButton != null) {
                NewCustomerButton.Dispose ();
                NewCustomerButton = null;
            }
        }
    }
}