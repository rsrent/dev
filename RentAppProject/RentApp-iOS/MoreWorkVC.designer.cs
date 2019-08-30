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
    [Register ("MoreWorkVC")]
    partial class MoreWorkVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DateToComplete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell DateToCompleteCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextView TaskDescription { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TaskType { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell TaskTypeCell { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel UserToComplete { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITableViewCell UserToCompleteCell { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DateToComplete != null) {
                DateToComplete.Dispose ();
                DateToComplete = null;
            }

            if (DateToCompleteCell != null) {
                DateToCompleteCell.Dispose ();
                DateToCompleteCell = null;
            }

            if (TaskDescription != null) {
                TaskDescription.Dispose ();
                TaskDescription = null;
            }

            if (TaskType != null) {
                TaskType.Dispose ();
                TaskType = null;
            }

            if (TaskTypeCell != null) {
                TaskTypeCell.Dispose ();
                TaskTypeCell = null;
            }

            if (UserToComplete != null) {
                UserToComplete.Dispose ();
                UserToComplete = null;
            }

            if (UserToCompleteCell != null) {
                UserToCompleteCell.Dispose ();
                UserToCompleteCell = null;
            }
        }
    }
}