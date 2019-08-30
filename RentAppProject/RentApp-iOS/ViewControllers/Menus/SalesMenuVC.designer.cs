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
    [Register ("SalesMenuVC")]
    partial class SalesMenuVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ViewCustomersButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ViewCustomersButton != null) {
                ViewCustomersButton.Dispose ();
                ViewCustomersButton = null;
            }
        }
    }
}