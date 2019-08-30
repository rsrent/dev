using System;
using Rent.Models;

namespace Rent.DTOs
{
    /*
    public class LocationUserDTO
    {
        public int UserID { get; set; }
        public int LocationID { get; set; }

        public string Title { get; set; }
        public string HourText { get; set; }

        public UserDTO User { get; set; }
        public LocationDTO Location { get; set; }


        void SetupBasics(LocationUser lu)
        {
            UserID = lu.UserID;
            LocationID = lu.LocationID;
            Title = lu.Title;
            HourText = lu.HourText;
        }

        public LocationUserDTO() { }

        public LocationUserDTO(LocationUser lu)
        {
            SetupBasics(lu);
            User = UserDTO.Basic(lu.User);
            Location = LocationDTO.Basic(lu.Location);
        }

        public static LocationUserDTO Basic(LocationUser lu)
        {
            if (lu == null)
                return null;
            var dto = new LocationUserDTO();
            dto.SetupBasics(lu);
            return dto;
        }


    }
    */
}
