using System;
using System.Linq.Expressions;

namespace Rent.Models.Projects
{
    public class Comment
    {
        public int ID { get; set; }
        public int ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }

        static public Expression<Func<Comment, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Title,
                v.Body,
            } : null;
        }
    }
}
