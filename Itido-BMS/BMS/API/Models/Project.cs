using System;
using System.Collections.Generic;

namespace API.Models
{
    public class Project : Unit
    {
        public long ClientId { get; set; }
        public long LocationId { get; set; }

        public virtual Client Client { get; set; }
        public virtual Location Location { get; set; }
    }
}
