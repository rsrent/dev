using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

namespace Rent.Models.Projects
{
    public class Project
    {
        public int ID { get; set; }
        public int? ParentID { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }

        public virtual Project Parent { get; set; }
        public virtual ICollection<ProjectItem> ProjectItems { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }
        public virtual ICollection<Project> Projects { get; set; }

        static public Expression<Func<Project, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Name,
                v.Path,
                //ChildrenCount = v.Projects != null ? v.Projects.Count : -1,
                //Parent = BasicDTO(requester, requesterRole).Compile()(v.Parent),
            } : null;
        }

        static public Expression<Func<Project, dynamic>> StandardDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Name,
                v.Path,
                IsClient = v.ProjectItems.Any(pi => pi.ProjectItemType == ProjectItemType.Client)
            } : null;
        }

        static public Expression<Func<Project, dynamic>> DetailedDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Name,
                v.Path,
                ChildrenCount = v.Projects != null ? v.Projects.Count : -1,
                IsClient = v.ProjectItems.Any(pi => pi.ProjectItemType == ProjectItemType.Client)
                //Parent = BasicDTO(requester, requesterRole).Compile()(v.Parent),
                //v.ProjectItems,
                //ProjectItems = v.ProjectItems != null ? (v.ProjectItems.Select(ProjectItem.BasicDTO(requester, requesterRole))) : null,
                //ProjectItems = v.ProjectItems.ToList().Select(ProjectItem.BasicDTO(requester, requesterRole)),
            } : null;
        }
    }
}
