using System;
using RentAppProject;

namespace RentApp.ViewModels
{
    public class EmployeeTableVM
    {
		public Action<User> SpecialAction { get; set; }
        public Location Location { get; set; }
    }
}
