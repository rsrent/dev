// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ModuleLibraryiOS.Chat
{
    [Register ("MessageCell")]
    partial class MessageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LeftImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LeftImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint LeftWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView MessageContainerView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RightImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RightImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint RightWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeLabelHeightConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LeftImage != null) {
                LeftImage.Dispose ();
                LeftImage = null;
            }

            if (LeftImageButton != null) {
                LeftImageButton.Dispose ();
                LeftImageButton = null;
            }

            if (LeftWidthConstraint != null) {
                LeftWidthConstraint.Dispose ();
                LeftWidthConstraint = null;
            }

            if (MessageContainerView != null) {
                MessageContainerView.Dispose ();
                MessageContainerView = null;
            }

            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }

            if (RightImage != null) {
                RightImage.Dispose ();
                RightImage = null;
            }

            if (RightImageButton != null) {
                RightImageButton.Dispose ();
                RightImageButton = null;
            }

            if (RightWidthConstraint != null) {
                RightWidthConstraint.Dispose ();
                RightWidthConstraint = null;
            }

            if (TimeLabel != null) {
                TimeLabel.Dispose ();
                TimeLabel = null;
            }

            if (TimeLabelHeightConstraint != null) {
                TimeLabelHeightConstraint.Dispose ();
                TimeLabelHeightConstraint = null;
            }
        }
    }
}