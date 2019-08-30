using System;
using System.Collections.Generic;
using static RentAppProject.Permission;

namespace RentAppProject
{
    public class User
    {
		public int ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        //public Role Role { get; set; }
        public string Title { get; set; }
        public string HourText { get; set; }
        public bool Disabled { get; set; }

        public string RoleName { get; set; }
        public string CustomerName { get; set; }

        public int? EmployeeNumber { get; set; }
        //public int RoleID { get; set; }
        public string Comment { get; set; }
        public List<UserPermission> Permissions { get; set; }
        public int? CustomerID { get; set; }
        public int? RoleID { get; set; }
        //public Customer Customer { get; set; }
        public string ImageLocation { get; set; }
        public byte[] ImageArray { get; set; }
    }
}
