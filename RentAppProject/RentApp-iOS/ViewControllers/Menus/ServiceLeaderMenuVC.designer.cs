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
    [Register ("ServiceLeaderMenuVC")]
    partial class ServiceLeaderMenuVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ViewCustomersButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ViewEmployeesButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ViewCustomersButton != null) {
                ViewCustomersButton.Dispose ();
                ViewCustomersButton = null;
            }

            if (ViewEmployeesButton != null) {
                ViewEmployeesButton.Dispose ();
                ViewEmployeesButton = null;
            }
        }
    }
}