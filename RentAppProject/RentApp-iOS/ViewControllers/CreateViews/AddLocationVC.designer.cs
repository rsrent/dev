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
    [Register ("AddLocationVC")]
    partial class AddLocationVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField AddressTF { get; set; }

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
        UIKit.UITextField IntervalOfServiceLeaderMeetingTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField NameTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField PhoneTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField ProjectNumberTF { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectCustomerContactButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton SelectServiceLeaderButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIStackView ServiceLeaderStack { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (AddressTF != null) {
                AddressTF.Dispose ();
                AddressTF = null;
            }

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

            if (IntervalOfServiceLeaderMeetingTF != null) {
                IntervalOfServiceLeaderMeetingTF.Dispose ();
                IntervalOfServiceLeaderMeetingTF = null;
            }

            if (NameTF != null) {
                NameTF.Dispose ();
                NameTF = null;
            }

            if (PhoneTF != null) {
                PhoneTF.Dispose ();
                PhoneTF = null;
            }

            if (ProjectNumberTF != null) {
                ProjectNumberTF.Dispose ();
                ProjectNumberTF = null;
            }

            if (SelectCustomerContactButton != null) {
                SelectCustomerContactButton.Dispose ();
                SelectCustomerContactButton = null;
            }

            if (SelectServiceLeaderButton != null) {
                SelectServiceLeaderButton.Dispose ();
                SelectServiceLeaderButton = null;
            }

            if (ServiceLeaderStack != null) {
                ServiceLeaderStack.Dispose ();
                ServiceLeaderStack = null;
            }
        }
    }
}