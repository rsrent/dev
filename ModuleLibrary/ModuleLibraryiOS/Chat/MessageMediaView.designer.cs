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

namespace ModuleLibraryiOS.Chat
{
    [Register ("MessageMediaView")]
    partial class MessageMediaView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint HeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton InspectElementButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton VideoButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (HeightConstraint != null) {
                HeightConstraint.Dispose ();
                HeightConstraint = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }

            if (InspectElementButton != null) {
                InspectElementButton.Dispose ();
                InspectElementButton = null;
            }

            if (VideoButton != null) {
                VideoButton.Dispose ();
                VideoButton = null;
            }
        }
    }
}