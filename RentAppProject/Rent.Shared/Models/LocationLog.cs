using System;
namespace Rent.Shared.Models
{
    public class LocationLog
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Log { get; set; }
        public DateTime DateCreated { get; set; }
    }
}