using System;
namespace Rent.Models
{
    public class LocationEconomy
    {
        public int ID { get; set; }
        //public int LocationID { get; set; }

        public float PriceRegularCleaning { get; set; }
        public float PriceWindowCleaning { get; set; }
        public PricePerHourCategory PricePerHourCategory { get; set; }
        public DateTime? StartDate { get; set; }
    }
}
