using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class MessageVideo : SpecialMessage
    {
        public string VideoLocator { get; set; }
        public string ThumbnailLocator { get; set; }

    }
}
