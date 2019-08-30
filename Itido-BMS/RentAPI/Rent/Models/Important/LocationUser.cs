using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Rent.Repositories;

namespace Rent.Models
{
    public class LocationUser : ProtectedObject
    {
        public int UserID { get; set; }
        public int LocationID { get; set; }
        public string Title { get; set; }
        public string HourText { get; set; }
        public virtual User User { get; set; }
        public virtual Location Location { get; set; }
        
        public dynamic ToLocation()
        {
            return Merger.Merge(Location.Basic(), new
            {
                UserID,
                Title = !string.IsNullOrEmpty(Title) ? Title : !string.IsNullOrEmpty(User?.Title) ? User.Title : User?.Role != null ? User.Role.Name : "",
                HourText,
            });
        }
        
        public dynamic ToUser()
        {
            return Merger.Merge(User.Basic(), new
            {
                UserID,
                Title = !string.IsNullOrEmpty(Title) ? Title : !string.IsNullOrEmpty(User.Title) ? User.Title : User.Role != null ? User.Role.Name : "",
                HourText,
            });
        }
    }
}
