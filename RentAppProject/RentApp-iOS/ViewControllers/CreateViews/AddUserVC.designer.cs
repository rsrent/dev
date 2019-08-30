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
    [Register ("AddUserVC")]
    partial class AddUserVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView CommentStack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView CommentTV { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField EmailTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField FirstNameTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField LastNameTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PhoneTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView RoleStack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView TitleLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CommentStack != null) {
                CommentStack.Dispose ();
                CommentStack = null;
            }

            if (CommentTV != null) {
                CommentTV.Dispose ();
                CommentTV = null;
            }

            if (EmailTF != null) {
                EmailTF.Dispose ();
                EmailTF = null;
            }

            if (FirstNameTF != null) {
                FirstNameTF.Dispose ();
                FirstNameTF = null;
            }

            if (ImageButton != null) {
                ImageButton.Dispose ();
                ImageButton = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (LastNameTF != null) {
                LastNameTF.Dispose ();
                LastNameTF = null;
            }

            if (PhoneTF != null) {
                PhoneTF.Dispose ();
                PhoneTF = null;
            }

            if (RoleStack != null) {
                RoleStack.Dispose ();
                RoleStack = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }
        }
    }
}