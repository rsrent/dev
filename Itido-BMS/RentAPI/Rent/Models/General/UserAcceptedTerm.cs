using System;
namespace Rent.Models
{
    public class UserAcceptedTerm
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public int TermID { get; set; }

        public virtual User User { get; set; }
        public virtual Term Term { get; set; }
    }
}
