﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class MessageMoreWork : SpecialMessage
    {
        public DateTime Time { get; set; }
        public Status Status { get; set; }
    }
}
