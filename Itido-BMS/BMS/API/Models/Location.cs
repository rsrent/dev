using System;
using API.Models;
namespace API.Models
{
    public class Location : Unit
    {
        public long ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
