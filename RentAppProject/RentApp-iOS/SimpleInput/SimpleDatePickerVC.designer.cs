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
    [Register ("SimpleDatePickerVC")]
    partial class SimpleDatePickerVC
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIDatePicker DatePicker { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton DoneButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TitelLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DatePicker != null) {
                DatePicker.Dispose ();
                DatePicker = null;
            }

            if (DoneButton != null) {
                DoneButton.Dispose ();
                DoneButton = null;
            }

            if (TitelLabel != null) {
                TitelLabel.Dispose ();
                TitelLabel = null;
            }
        }
    }
}