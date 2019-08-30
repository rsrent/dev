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
    [Register ("CustomerMenuVC")]
    partial class CustomerMenuVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CommentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView CommentStack { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel ContactLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DocumentsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton EconomyButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitleLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ViewLocationsButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CommentLabel != null) {
                CommentLabel.Dispose ();
                CommentLabel = null;
            }

            if (CommentStack != null) {
                CommentStack.Dispose ();
                CommentStack = null;
            }

            if (ContactLabel != null) {
                ContactLabel.Dispose ();
                ContactLabel = null;
            }

            if (DocumentsButton != null) {
                DocumentsButton.Dispose ();
                DocumentsButton = null;
            }

            if (EconomyButton != null) {
                EconomyButton.Dispose ();
                EconomyButton = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (TitleLabel != null) {
                TitleLabel.Dispose ();
                TitleLabel = null;
            }

            if (ViewLocationsButton != null) {
                ViewLocationsButton.Dispose ();
                ViewLocationsButton = null;
            }
        }
    }
}