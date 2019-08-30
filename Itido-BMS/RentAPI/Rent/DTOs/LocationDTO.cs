using Rent.Data;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    /*
    public class LocationDTO
    {
        public int ID { get; set; }
        public int? CustomerID { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Comment { get; set; }
        public string ImageLocation { get; set; }
        public string Phone { get; set; }
        public int? ProjectNumber { get; set; }
        public int IntervalOfServiceLeaderMeeting { get; set; }
        public int GeneralFolderID { get; set; }
        public CustomerDTO Customer { get; set; }
        public UserDTO CustomerContact { get; set; }
        public UserDTO ServiceLeader { get; set; }
        public int CleaningplanFolderID { get; set; }
        public bool Disabled { get; set; }
        public bool HoursCompleted { get; set; }

        public QualityReport LastQualityReport { get; set; }
        public DateTime NextQualityReport { get; set; }

        //public ICollection<LocationUser> LocationUsers { get; set; }



        void SetupBasics(Location l)
        {
            ID = l.ID;
            CustomerID = l.CustomerID;
            Name = l.Name;
            Address = l.Address;
            Comment = l.Comment;
            ImageLocation = l.ImageLocation;
            ProjectNumber = l.ProjectNumber;
            GeneralFolderID = l.GeneralFolderID;
            IntervalOfServiceLeaderMeeting = l.IntervalOfServiceLeaderMeeting;
            Phone = l.Phone;
            Disabled = l.Disabled;
            CleaningplanFolderID = l.CleaningplanFolderID;
        }

        public LocationDTO() { }

        public LocationDTO(Location l, User t1, User t2)
        {
            SetupBasics(l);
            Customer = CustomerDTO.Basic(l.Customer);
            CustomerContact = UserDTO.Basic(l.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 8).User);
            ServiceLeader = UserDTO.Basic(l.LocationUsers.FirstOrDefault(lu => lu.User.RoleID == 3).User);

            if(l.QualityReports != null && l.QualityReports.Any())
                LastQualityReport = new QualityReport { Time = l.QualityReports.Last().Time };
            if(LastQualityReport != null)
                NextQualityReport = LastQualityReport.Time.AddDays(IntervalOfServiceLeaderMeeting);
        }

        public static LocationDTO Basic(Location l)
        {
            if (l == null)
                return null;
            var dto = new LocationDTO();
            dto.SetupBasics(l);
            return dto;
        }


    }
    */
}
