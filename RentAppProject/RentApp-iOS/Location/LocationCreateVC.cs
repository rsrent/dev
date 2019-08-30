using Foundation;
using System;
using UIKit;
using RentAppProject;
using ModuleLibraryiOS.Navigation;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace RentApp
{
    public partial class LocationCreateVC : UITableViewController
    {
        LocationRepository _locationRepository = AppDelegate.ServiceProvider.GetService<LocationRepository>();
        Location _location;

        public LocationCreateVC (IntPtr handle) : base (handle) { }

        public void NewLocation()
        {
            _location = new Location();
            this.RightNavigationButton("Opret", () => {

            });
        }

        public void EditLocation(Location customer)
        {
            _location = customer;
            this.RightNavigationButton("Opdater", () => {

            });
        }

        async Task<Location> GetLocation()
        {
            _location.Name = Name.Text;
            _location.Comment = Comment.Text;

            return _location;
        }
    }
}