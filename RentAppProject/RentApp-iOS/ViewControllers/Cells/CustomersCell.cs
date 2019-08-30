using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class CustomersCell : UITableViewCell
    {
        public CustomersCell(IntPtr handle) : base(handle)
        {
        }

        public void UpdateCell(Customer customer) {
            Text.Text = customer.Name;

            if (customer.Disabled)
                BackgroundColor = UIColor.FromRGB(255, 176, 147);
            else BackgroundColor = UIColor.White;
        }
    }
}