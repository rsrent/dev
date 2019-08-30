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
    [Register ("MessageTextView")]
    partial class MessageTextView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BackgroundButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel MessageLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BackgroundButton != null) {
                BackgroundButton.Dispose ();
                BackgroundButton = null;
            }

            if (MessageLabel != null) {
                MessageLabel.Dispose ();
                MessageLabel = null;
            }
        }
    }
}