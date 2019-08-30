using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class LocationCell : UITableViewCell
    {
        public LocationCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(Location location)
        {
            Text.Text = "";
			if (location.CustomerName != null) Text.Text = location.CustomerName + ", ";
            Text.Text += location.Name;


            if (location.Disabled)
                BackgroundColor = UIColor.FromRGB(255, 176, 147);
            else if (location.HoursCompleted)
                BackgroundColor = UIColor.FromRGB(202, 255, 196);
            else
                BackgroundColor = UIColor.White;
        }
    }
}