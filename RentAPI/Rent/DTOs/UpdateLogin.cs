using System;
using Rent.Models;

namespace Rent.DTOs
{
    public class UpdateLogin
    {
        public Login Login { get; set; }
        public string NewPassword { get; set; }
    }
}
