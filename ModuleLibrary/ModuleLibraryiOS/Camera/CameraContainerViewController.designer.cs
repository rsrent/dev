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

namespace ModuleLibraryiOS.Camera
{
    [Register ("CameraContainerViewController")]
    partial class CameraContainerViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton AlbumButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView CameraView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CancelButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SwapButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton TakePictureButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UseButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView VideoView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AlbumButton != null) {
                AlbumButton.Dispose ();
                AlbumButton = null;
            }

            if (BackButton != null) {
                BackButton.Dispose ();
                BackButton = null;
            }

            if (CameraView != null) {
                CameraView.Dispose ();
                CameraView = null;
            }

            if (CancelButton != null) {
                CancelButton.Dispose ();
                CancelButton = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (SwapButton != null) {
                SwapButton.Dispose ();
                SwapButton = null;
            }

            if (TakePictureButton != null) {
                TakePictureButton.Dispose ();
                TakePictureButton = null;
            }

            if (UseButton != null) {
                UseButton.Dispose ();
                UseButton = null;
            }

            if (VideoView != null) {
                VideoView.Dispose ();
                VideoView = null;
            }
        }
    }
}