using Microsoft.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.DTOs
{
    public class OutlookEmailSendDTO : OutlookDTO
    {
        public override string Token { get; set; }
        public override string Email { get; set; }
        public string Subject { get; set; }
        public BodyType ContentType { get; set; }
        public string Content { get; set; }
        public List<Recipient> ToRecipients { get; set; }
    }

}
