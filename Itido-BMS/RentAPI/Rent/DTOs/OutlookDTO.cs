using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public abstract class OutlookDTO
    {
        public abstract string Token { get; set; }
        public abstract string Email { get; set; }

    }
}
