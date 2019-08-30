using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.Storage;
using Microsoft.Extensions.DependencyInjection;
using ModuleLibraryiOS.Image;
using RentApp.Repository;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class EmployeeCell : UITableViewCell
    {
        StorageRepository _storage = AppDelegate.ServiceProvider.GetService<StorageRepository>();

        public EmployeeCell (IntPtr handle) : base (handle)
        {
        }

        string currentImageLocator = "";
        public void UpdateCell(User user) {
            Image.Image = null;
            Image.Round();
            NameLabel.Text = user.FirstName + " " + user.LastName;

            RoleLabel.Text = user.Title;
            currentImageLocator = user.ImageLocation;

            if (!string.IsNullOrEmpty(user.HourText))
                HourLabel.Text = user.HourText;
            else HourLabel.Text = "";

            if (!string.IsNullOrEmpty(user.CustomerName))
                NameLabel.Text += ", " + user.CustomerName;
            /*
            if(user.Role != null) {
				RoleLabel.Text = user.Role.Name;
            }

            if(user.Customer != null) {
                RoleLabel.Text += ", " + user.Customer.Name;
            } */

            if (user.Disabled)
                BackgroundColor = UIColor.FromRGB(255, 176, 147);
            else BackgroundColor = UIColor.White;

            LoadImage(user);
        }

        async Task LoadImage(User user)
        {
            if (!string.IsNullOrEmpty(user.ImageLocation))
            {
                var img = await _storage.DownloadImage(user.ImageLocation, 50);
                if (user.ImageLocation.Equals(currentImageLocator))
                    Image.Image = img;
            }
        }
    }
}