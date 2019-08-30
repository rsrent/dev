using System;

using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Table
{
    public partial class TextCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("TextCell");
        public static readonly UINib Nib;

        static TextCell()
        {
            Nib = UINib.FromName("TextCell", NSBundle.MainBundle);
        }

        protected TextCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public TextCell(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {
		}

		public void UpdateCell(string text)
		{
            Label.Text = text;
		}
    }
}
