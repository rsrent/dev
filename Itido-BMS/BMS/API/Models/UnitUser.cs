namespace API.Models
{
    public class UnitUser
    {
        public long UserId {get; set;}

        public long UnitId {get; set;}

        public virtual Unit Unit { get; set; }
        public virtual User User { get; set; }
    }



}