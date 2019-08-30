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
    [Register ("LocationCreateVC")]
    partial class LocationCreateVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Address { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Comment { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LocationManager { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Name { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Phone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ProjectNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ServiceLeader { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ServiceLeaderInterval { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Address != null) {
                Address.Dispose ();
                Address = null;
            }

            if (Comment != null) {
                Comment.Dispose ();
                Comment = null;
            }

            if (LocationManager != null) {
                LocationManager.Dispose ();
                LocationManager = null;
            }

            if (Name != null) {
                Name.Dispose ();
                Name = null;
            }

            if (Phone != null) {
                Phone.Dispose ();
                Phone = null;
            }

            if (ProjectNumber != null) {
                ProjectNumber.Dispose ();
                ProjectNumber = null;
            }

            if (ServiceLeader != null) {
                ServiceLeader.Dispose ();
                ServiceLeader = null;
            }

            if (ServiceLeaderInterval != null) {
                ServiceLeaderInterval.Dispose ();
                ServiceLeaderInterval = null;
            }
        }
    }
}