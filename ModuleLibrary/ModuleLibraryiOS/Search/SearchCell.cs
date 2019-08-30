using System;

using Foundation;
using UIKit;

namespace ModuleLibraryiOS.Search
{
    public partial class SearchCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("SearchCell");
        public static readonly UINib Nib;

        static SearchCell()
        {
            Nib = UINib.FromName("SearchCell", NSBundle.MainBundle);
        }

        protected SearchCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

		public SearchCell(NSString cellId) : base (UITableViewCellStyle.Default, cellId)
        {

		}

        public void UpdateCell(SearchType tableItem, bool added) {
			var color = new UIColor(0.7f, 0.8f, 0.9f, 1f);
			Label.Text = tableItem.GetText();
			if (added)
			{
				BackgroundColor = color;
				Label.TextColor = UIColor.White;
			}
			else
			{
				BackgroundColor = UIColor.White;
				Label.TextColor = color;
			}
        }
    }
}
