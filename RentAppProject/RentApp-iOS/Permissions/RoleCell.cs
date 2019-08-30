using Foundation;
using System;
using UIKit;
using RentAppProject;

namespace RentApp
{
    public partial class RoleCell : UITableViewCell
    {
        public RoleCell (IntPtr handle) : base (handle)
        {
        }

        public void UpdateCell(Role role)
        {
            Text.Text = role.Name;
        }
    }
}