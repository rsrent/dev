using System;
namespace Rent.Models.Projects
{
    public class ProjectItemUser
    {
        public int ProjectItemID { get; set; }
        public int UserID { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public virtual User User { get; set; }
    }
}
