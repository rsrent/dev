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
        public Role Role { get; set; }
        //public int RoleID { get; set; }
        public string Comment { get; set; }
        public List<UserPermission> Permissions { get; set; }
        public int? CustomerID { get; set; }
        public Customer Customer { get; set; }
        public string ImageLocation { get; set; }
    }
}
