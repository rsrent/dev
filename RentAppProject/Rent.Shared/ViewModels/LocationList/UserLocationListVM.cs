using System;
using System.Collections.Generic;
using RentApp;
using RentAppProject;

namespace Rent.Shared.ViewModels
{
    public class UserLocationListVM : ILocationListVM
    {
        public UserLocationListVM(LocationRepository locationRepository) : base(locationRepository) { }

        User User;

        public void SetUser(User user)
        {
            User = user;
        }

        public override async void GetLocations(Action<List<Location>> Success)
        {
            /*
            await _locationRepository.GetForUser(User, (obj) => {
                SetLocations(obj);
                Success(obj);
            });
            */
        }
    }
}
