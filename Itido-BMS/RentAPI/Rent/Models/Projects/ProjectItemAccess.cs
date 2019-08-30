using System;
using Rent.Models.TimePlanning;

namespace Rent.Models.Projects
{
    public class ProjectItemAccess
    {
        public int ProjectItemID { get; set; }
        public int ProjectRoleID { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
