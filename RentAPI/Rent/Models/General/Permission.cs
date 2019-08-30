using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class Permission
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public bool OnlyMasterCanChange { get; set; }
    }
}
