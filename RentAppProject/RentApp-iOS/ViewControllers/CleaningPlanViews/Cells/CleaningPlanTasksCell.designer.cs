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
    [Register ("CleaningPlanTasksCell")]
    partial class CleaningPlanTasksCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FrequenzyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LastCleanedLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel TimesCleanedLabel { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (FrequenzyLabel != null) {
                FrequenzyLabel.Dispose ();
                FrequenzyLabel = null;
            }

            if (LastCleanedLabel != null) {
                LastCleanedLabel.Dispose ();
                LastCleanedLabel = null;
            }

            if (TimesCleanedLabel != null) {
                TimesCleanedLabel.Dispose ();
                TimesCleanedLabel = null;
            }
        }
    }
}