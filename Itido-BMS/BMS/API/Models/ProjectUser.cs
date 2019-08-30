using System;
namespace API.Models
{
    public class ProjectUser
    {
        public long ProjectId { get; set; }
        public long UserId { get; set; }
        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}