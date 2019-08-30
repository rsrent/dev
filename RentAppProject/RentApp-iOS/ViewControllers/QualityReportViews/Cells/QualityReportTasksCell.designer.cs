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
    [Register ("QualityReportTasksCell")]
    partial class QualityReportTasksCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton BadButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BadImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GoodButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView GoodImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView NeutralImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton OkayButton { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BadButton != null) {
                BadButton.Dispose ();
                BadButton = null;
            }

            if (BadImage != null) {
                BadImage.Dispose ();
                BadImage = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (GoodButton != null) {
                GoodButton.Dispose ();
                GoodButton = null;
            }

            if (GoodImage != null) {
                GoodImage.Dispose ();
                GoodImage = null;
            }

            if (ImageButton != null) {
                ImageButton.Dispose ();
                ImageButton = null;
            }

            if (NeutralImage != null) {
                NeutralImage.Dispose ();
                NeutralImage = null;
            }

            if (OkayButton != null) {
                OkayButton.Dispose ();
                OkayButton = null;
            }
        }
    }
}