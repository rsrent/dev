using System;
using Rent.Models.Projects;

namespace Rent.Models
{
    public class LocationLog
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Log { get; set; }

        public int? LocationID { get; set; }
        public int UserID { get; set; }
        public DateTime DateCreated { get; set; }

        public Location Location { get; set; }
        public User User { get; set; }
        public bool CustomerCreated { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
    }
}
