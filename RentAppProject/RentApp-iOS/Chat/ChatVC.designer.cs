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
    [Register ("ChatVC")]
    partial class ChatVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint BottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ChatFunctionsButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView ChatFunctionsView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ChatFunctionsViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint ChatFunctionsViewWidthConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        ModuleLibraryiOS.Chat.UITextViewPlaceholder MessageTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SendButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableView Table { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BottomConstraint != null) {
                BottomConstraint.Dispose ();
                BottomConstraint = null;
            }

            if (ChatFunctionsButton != null) {
                ChatFunctionsButton.Dispose ();
                ChatFunctionsButton = null;
            }

            if (ChatFunctionsView != null) {
                ChatFunctionsView.Dispose ();
                ChatFunctionsView = null;
            }

            if (ChatFunctionsViewHeightConstraint != null) {
                ChatFunctionsViewHeightConstraint.Dispose ();
                ChatFunctionsViewHeightConstraint = null;
            }

            if (ChatFunctionsViewWidthConstraint != null) {
                ChatFunctionsViewWidthConstraint.Dispose ();
                ChatFunctionsViewWidthConstraint = null;
            }

            if (MessageTF != null) {
                MessageTF.Dispose ();
                MessageTF = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (Table != null) {
                Table.Dispose ();
                Table = null;
            }
        }
    }
}