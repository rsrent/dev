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
    [Register ("AddCustomerVC")]
    partial class AddCustomerVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView CommentStack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView CommentTV { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NameTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectCustomerResponsible { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectHRContact { get; set; }

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

            if (ImageButton != null) {
                ImageButton.Dispose ();
                ImageButton = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (NameTF != null) {
                NameTF.Dispose ();
                NameTF = null;
            }

            if (SelectCustomerResponsible != null) {
                SelectCustomerResponsible.Dispose ();
                SelectCustomerResponsible = null;
            }

            if (SelectHRContact != null) {
                SelectHRContact.Dispose ();
                SelectHRContact = null;
            }
        }
    }
}