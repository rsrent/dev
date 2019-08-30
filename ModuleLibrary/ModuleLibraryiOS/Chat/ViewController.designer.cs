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
    [Register ("ChatViewController")]
    partial class ChatViewController
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
        UIKit.UISearchBar SearchBar { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView SearchView { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SearchViewHeightConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SendButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.NSLayoutConstraint SendButtonBottomConstraint { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIView TableContainer { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UITextViewPlaceholder TextField { get; set; }

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

            if (SearchBar != null) {
                SearchBar.Dispose ();
                SearchBar = null;
            }

            if (SearchView != null) {
                SearchView.Dispose ();
                SearchView = null;
            }

            if (SearchViewHeightConstraint != null) {
                SearchViewHeightConstraint.Dispose ();
                SearchViewHeightConstraint = null;
            }

            if (SendButton != null) {
                SendButton.Dispose ();
                SendButton = null;
            }

            if (SendButtonBottomConstraint != null) {
                SendButtonBottomConstraint.Dispose ();
                SendButtonBottomConstraint = null;
            }

            if (TableContainer != null) {
                TableContainer.Dispose ();
                TableContainer = null;
            }

            if (TextField != null) {
                TextField.Dispose ();
                TextField = null;
            }
        }
    }
}