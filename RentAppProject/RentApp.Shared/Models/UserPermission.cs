using System;
namespace RentAppProject
{
    public class UserPermission
    {
        public int UserID { get; set; }
        public int PermissionID { get; set; }
        public string Name { get; set; }
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }
        public bool Decide { get; set; }
	}
}
