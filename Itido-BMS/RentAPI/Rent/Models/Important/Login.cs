using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Login : ProtectedObject
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public DateTime PasswordLastUpdated { get; set; }
    }
}
