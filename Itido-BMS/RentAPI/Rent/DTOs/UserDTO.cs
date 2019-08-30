using System;
using System.Collections.Generic;
using Rent.Models;

namespace Rent.DTOs
{
    /*
    public class UserDTO
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public int? CustomerID { get; set; }
        public int? EmployeeNumber { get; set; }
        public string Title { get; set; }
        public string HourText { get; set; }
        public bool Disabled { get; set; }
        public Role Role { get; set; }
        public CustomerDTO Customer { get; set; }

        public ICollection<UserPermissionsDTO> Permissions { get; set; }
        public ICollection<LocationUser> LocationUsers { get; set; }

        void SetupBasics(User u)
        {
            ID = u.ID;
            FirstName = u.FirstName;
            LastName = u.LastName;
            Phone = u.Phone;
            Email = u.Email;
            Comment = u.Comment;
            CustomerID = u.CustomerID;
            ImageLocation = u.ImageLocation;
            Role = u.Role;
            EmployeeNumber = u.EmployeeNumber;
            Title = !string.IsNullOrEmpty(u.Title) ? u.Title : (u.Role?.Name);
            Disabled = u.Disabled;
        }

        public UserDTO() { }

        public UserDTO(User u)
        {
            Customer = CustomerDTO.Basic(u.Customer);
            Title = !string.IsNullOrEmpty(u.Title) ? u.Title : Role.Name;
        }

        public UserDTO(LocationUser lu)
        {
            SetupBasics(lu.User);
            Customer = CustomerDTO.Basic(lu.User.Customer);
            Title = !string.IsNullOrEmpty(lu.Title) ? lu.Title : !string.IsNullOrEmpty(lu.User.Title) ? lu.User.Title : Role.Name;
            HourText = lu.HourText;
        }

        public static UserDTO Basic(User u)
        {
            if (u == null)
                return null;
            var dto = new UserDTO();
            dto.SetupBasics(u);
            return dto;
        }


    }
    */
}
