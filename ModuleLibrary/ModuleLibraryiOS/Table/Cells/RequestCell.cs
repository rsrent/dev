using System;

using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public partial class RequestCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("RequestCell");
        public static readonly UINib Nib;

        static RequestCell()
        {
            Nib = UINib.FromName("RequestCell", NSBundle.MainBundle);
        }

        protected RequestCell(IntPtr handle) : base(handle)
        {
            Transform = CoreGraphics.CGAffineTransform.MakeRotation(3.14159f);
        }

		public RequestCell(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {
		}
    }
}
