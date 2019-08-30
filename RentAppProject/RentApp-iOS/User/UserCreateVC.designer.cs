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
    [Register ("UserCreateVC")]
    partial class UserCreateVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Customer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell CustomerCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView Description { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Email { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EmployeeNumber { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FirstName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView Image { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LastName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField Phone { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel Role { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell UserCellRole { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell UserCellUserNumber { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (Customer != null) {
                Customer.Dispose ();
                Customer = null;
            }

            if (CustomerCell != null) {
                CustomerCell.Dispose ();
                CustomerCell = null;
            }

            if (Description != null) {
                Description.Dispose ();
                Description = null;
            }

            if (Email != null) {
                Email.Dispose ();
                Email = null;
            }

            if (EmployeeNumber != null) {
                EmployeeNumber.Dispose ();
                EmployeeNumber = null;
            }

            if (FirstName != null) {
                FirstName.Dispose ();
                FirstName = null;
            }

            if (Image != null) {
                Image.Dispose ();
                Image = null;
            }

            if (LastName != null) {
                LastName.Dispose ();
                LastName = null;
            }

            if (Phone != null) {
                Phone.Dispose ();
                Phone = null;
            }

            if (Role != null) {
                Role.Dispose ();
                Role = null;
            }

            if (UserCellRole != null) {
                UserCellRole.Dispose ();
                UserCellRole = null;
            }

            if (UserCellUserNumber != null) {
                UserCellUserNumber.Dispose ();
                UserCellUserNumber = null;
            }
        }
    }
}