using Foundation;
using System;
using UIKit;

namespace RentApp
{
    public partial class SpecialFunctionsCell : UITableViewCell
    {
        public SpecialFunctionsCell (IntPtr handle) : base (handle)
        {
            TextLabel.TextColor = UIColor.FromName("ThemeColor");
        }
    }
}