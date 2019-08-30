using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class OutlookEventDTO : OutlookDTO
    {
        public override string Token { get; set; }
        public override string Email { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string TimeZone { get; set; }
        public string Subject { get; set; }
    }
}
