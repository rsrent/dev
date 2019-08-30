using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class BlackListedToken
    {
        public int ID { get; set; }
        [Index]
        public string TokenGuid { get; set; } // It can be quite difficult to get the actual JWT. Instead we save a unique GUID in the Jti field and use that.
        public DateTime BlackListTime { get; set; }
        public string IP { get; set; }
        [ForeignKey("Login")]
        public int LoginID { get; set; }

        public virtual Login Login { get; set; }
    }
}
