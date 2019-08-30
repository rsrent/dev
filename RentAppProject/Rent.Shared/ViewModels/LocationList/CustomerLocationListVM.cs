using System;
using System.Collections.Generic;
using RentApp;
using RentAppProject;

namespace Rent.Shared.ViewModels
{
    public class CustomerLocationListVM : ILocationListVM
    {
		public CustomerLocationListVM(LocationRepository locationRepository) : base(locationRepository) { }

		Customer Customer;

        public void SetCustomer(Customer customer) 
        {
            Customer = customer;
        }

        public override async void GetLocations(Action<List<Location>> Success)
        {
            await _locationRepository.GetForCustomer(Customer, (obj) => {
                SetLocations(obj);
                Success(obj);
            });
        }
    }
}
