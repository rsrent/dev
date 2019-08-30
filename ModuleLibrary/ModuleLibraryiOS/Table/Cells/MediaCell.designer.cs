// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace ModuleLibraryiOS.Table
{
    [Register ("MediaCell")]
    partial class MediaCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomSpaceConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView LeftImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton LeftImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView RightImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton RightImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint RoundButtonBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimeLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint TimeLabelHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VideoView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomSpaceConstraint != null) {
                BottomSpaceConstraint.Dispose ();
                BottomSpaceConstraint = null;
            }

            if (HeightConstraint != null) {
                HeightConstraint.Dispose ();
                HeightConstraint = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (LeftImage != null) {
                LeftImage.Dispose ();
                LeftImage = null;
            }

            if (LeftImageButton != null) {
                LeftImageButton.Dispose ();
                LeftImageButton = null;
            }

            if (RightImage != null) {
                RightImage.Dispose ();
                RightImage = null;
            }

            if (RightImageButton != null) {
                RightImageButton.Dispose ();
                RightImageButton = null;
            }

            if (RoundButtonBottomConstraint != null) {
                RoundButtonBottomConstraint.Dispose ();
                RoundButtonBottomConstraint = null;
            }

            if (TimeLabel != null) {
                TimeLabel.Dispose ();
                TimeLabel = null;
            }

            if (TimeLabelHeightConstraint != null) {
                TimeLabelHeightConstraint.Dispose ();
                TimeLabelHeightConstraint = null;
            }

            if (VideoView != null) {
                VideoView.Dispose ();
                VideoView = null;
            }
        }
    }
}