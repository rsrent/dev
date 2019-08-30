using System;
namespace RentAppProject
{
    public class Location
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int? ProjectNumber { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public int IntervalOfServiceLeaderMeeting { get; set; }
        public Customer Customer { get; set; }

        public int DocumentFolderID { get; set; }

        //public User MainUser { get; set; }
        public User CustomerContact { get; set; }
        public User ServiceLeader { get; set; }

        //public int? CustomerContactID { get; set; }
        //public int? ServiceLeaderID { get; set; }
    }
}
