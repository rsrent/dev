using System;
namespace Rent.Models.Projects
{
    public class ProjectUser
    {
        public int ProjectID { get; set; }
        public int UserID { get; set; }

        public virtual Project Project { get; set; }
        public virtual User User { get; set; }
    }
}
