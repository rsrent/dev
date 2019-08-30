using System;
namespace Rent.Models
{
    public class Term
    {
        public int ID { get; set; }
        public DateTime ValidFrom { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatedByUserID { get; set; }
    }
}
