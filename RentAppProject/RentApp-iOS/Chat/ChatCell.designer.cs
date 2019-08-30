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
    [Register ("ChatCell")]
    partial class ChatCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView BackgroundBox { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint LeftGreaterSpaceConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint LeftSpaceConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ReceivedBackground { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ReceivedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint RightGreaterSpaceConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint RightSpaceConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView SendedImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel SenderName { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SenderNameHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TextLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeLabelHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeLabelTopConstraint { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundBox != null) {
                BackgroundBox.Dispose ();
                BackgroundBox = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (LeftGreaterSpaceConstraint != null) {
                LeftGreaterSpaceConstraint.Dispose ();
                LeftGreaterSpaceConstraint = null;
            }

            if (LeftSpaceConstraint != null) {
                LeftSpaceConstraint.Dispose ();
                LeftSpaceConstraint = null;
            }

            if (ReceivedBackground != null) {
                ReceivedBackground.Dispose ();
                ReceivedBackground = null;
            }

            if (ReceivedImage != null) {
                ReceivedImage.Dispose ();
                ReceivedImage = null;
            }

            if (RightGreaterSpaceConstraint != null) {
                RightGreaterSpaceConstraint.Dispose ();
                RightGreaterSpaceConstraint = null;
            }

            if (RightSpaceConstraint != null) {
                RightSpaceConstraint.Dispose ();
                RightSpaceConstraint = null;
            }

            if (SendedImage != null) {
                SendedImage.Dispose ();
                SendedImage = null;
            }

            if (SenderName != null) {
                SenderName.Dispose ();
                SenderName = null;
            }

            if (SenderNameHeightConstraint != null) {
                SenderNameHeightConstraint.Dispose ();
                SenderNameHeightConstraint = null;
            }

            if (TextLabel != null) {
                TextLabel.Dispose ();
                TextLabel = null;
            }

            if (TimeLabel != null) {
                TimeLabel.Dispose ();
                TimeLabel = null;
            }

            if (TimeLabelHeightConstraint != null) {
                TimeLabelHeightConstraint.Dispose ();
                TimeLabelHeightConstraint = null;
            }

            if (TimeLabelTopConstraint != null) {
                TimeLabelTopConstraint.Dispose ();
                TimeLabelTopConstraint = null;
            }
        }
    }
}