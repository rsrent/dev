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
    [Register ("QualityReportTasksTestCell")]
    partial class QualityReportTasksTestCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView BadImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel CommentLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel DescriptionLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel FrequenzyLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView GoodImage { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel NameLabel { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView NeutralImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (BadImage != null) {
                BadImage.Dispose ();
                BadImage = null;
            }

            if (CommentLabel != null) {
                CommentLabel.Dispose ();
                CommentLabel = null;
            }

            if (DescriptionLabel != null) {
                DescriptionLabel.Dispose ();
                DescriptionLabel = null;
            }

            if (FrequenzyLabel != null) {
                FrequenzyLabel.Dispose ();
                FrequenzyLabel = null;
            }

            if (GoodImage != null) {
                GoodImage.Dispose ();
                GoodImage = null;
            }

            if (ImageButton != null) {
                ImageButton.Dispose ();
                ImageButton = null;
            }

            if (NameLabel != null) {
                NameLabel.Dispose ();
                NameLabel = null;
            }

            if (NeutralImage != null) {
                NeutralImage.Dispose ();
                NeutralImage = null;
            }
        }
    }
}