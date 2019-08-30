using System;
using System.Collections.Generic;
using System.Dynamic;
using Rent.DTOs;
using Rent.Repositories;

namespace Rent.Models
{
    public class User : IDto
    {
        public int ID { get; set; }
        public bool Disabled { get; set; }
        public int LoginID { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int RoleID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Comment { get; set; }
        public string Title { get; set; }
        public string ImageLocation { get; set; }
        public int? CustomerID { get; set; }
        public int? EmployeeNumber { get; set; }
        
        public virtual Role Role { get; set; }
        public virtual Login Login { get; set; }
        public Customer Customer { get; set; }
        public virtual ICollection<LocationUser> LocationUsers { get; set; }

        public override dynamic Detailed()
        {
            return Merger.Merge(new
            {
                RoleID,
                EmployeeNumber,
                Email,
                Phone,
                Comment,
            }, Basic());
        }
        
        public override dynamic Basic()
        {
            return new
            {
                ID,
                FirstName,
                LastName,
                RoleID,
                RoleName = Role?.Name,
                Title,
                CustomerName = Customer?.Name,
                ImageLocation,
                Disabled,
                CustomerID,
            };
        }
    }
}
