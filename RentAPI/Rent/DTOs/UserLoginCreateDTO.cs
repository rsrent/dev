using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class UserLoginCreateDTO
    {
        public User User { get; set; }
        public Login Login { get; set; }
        //public int CustomerID { get; set; }
        //public string TemplateRole { get; set; }
    }
}
