using System;
using System.Collections.Generic;
using Rent.Models.Projects;

namespace Rent.Models.TimePlanning
{
    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public DateTime SendTime { get; set; }
        public virtual User User { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
    }
}
