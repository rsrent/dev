using System;

using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public partial class RoundImageTitleDescriptionCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("RoundImageTitleDescriptionCell");
        public static readonly UINib Nib;

        static RoundImageTitleDescriptionCell()
        {
            Nib = UINib.FromName("RoundImageTitleDescriptionCell", NSBundle.MainBundle);
        }

        protected RoundImageTitleDescriptionCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public RoundImageTitleDescriptionCell(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {
		}

		public void UpdateCell()
		{
			ProfileRoundButton.Layer.BorderColor = UIColor.White.CGColor;
		}
    }
}
