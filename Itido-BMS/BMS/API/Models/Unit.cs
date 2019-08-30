using System;
using System.Collections.Generic;

namespace API.Models
{
    public class Unit
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<UnitUser> UnitUsers { get; set; }
    }
}
