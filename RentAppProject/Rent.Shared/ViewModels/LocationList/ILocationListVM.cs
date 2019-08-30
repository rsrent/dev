using System;
using System.Collections.Generic;
using System.Linq;
using RentApp;
using RentAppProject;

namespace Rent.Shared.ViewModels
{
    public abstract class ILocationListVM
    {
        public List<Location> Locations { get; private set; }

        public LocationRepository _locationRepository;
        public ILocationListVM(LocationRepository locationRepository)
        {
            _locationRepository = locationRepository;
        }

        public void SetLocations(List<Location> locations) {
            Locations = locations;
        }

        public abstract void GetLocations(Action<List<Location>> Success);

        public void SearchLocations(Action<List<Location>> Success, string searchText)
        {
            Success(Locations.Where(l => (l.Name + " " + l.CustomerName)
                                    .ToLower().Contains(searchText.ToLower())).ToList());
        }
    }
}
