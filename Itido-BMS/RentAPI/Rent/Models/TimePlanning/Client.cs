using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rent.Models.Projects;

namespace Rent.Models.TimePlanning
{
    public class Client
    {
        public int ID { get; set; }
        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public static Expression<Func<Client, dynamic>> StandardDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.ProjectItem.Project.Name,
            } : null;
        }
    }
}
