
namespace Rent.Models.TimePlanning
{
    public class WorkInvitation
    {
        public int ID { get; set; }
        public int ContractID { get; set; }
        public int WorkID { get; set; }
        public virtual Work Work { get; set; }
        public virtual Contract Contract { get; set; }
    }
}