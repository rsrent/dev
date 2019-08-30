using System;
using Rent.Models.TimePlanning;

namespace Rent.Models.Projects
{
    public class ProjectItemAccessTemplate
    {
        public ProjectItemType ProjectItemType { get; set; }
        public int ProjectRoleID { get; set; }
        public bool Read { get; set; }
        public bool Write { get; set; }
        public virtual ProjectRole ProjectRole { get; set; }
    }
}
