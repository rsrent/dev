using System;
namespace RentAppProject
{
    public class Customer
    {
        public int ID { get; set; }
        public string Name { get; set; }
		public User SalesRep { get; set; }
        public User MainUser { get; set; }
        public User KeyAccountManager { get; set; }

        //public int? SalesRepID { get; set; }
        //public int? MainUserID { get; set; }
        //public int? HRContactID { get; set; }

        public int DocumentFolderID { get; set; }
        public DateTime Created { get; set; }

		public CustomerStatus Status { get; set; }
		public string Comment { get; set; }
        public string ImageLocation { get; set; }

		public enum CustomerStatus
		{
			Lead, Customer, DeadLead, Terminated
		}
    }
}
