using System;
using System.Linq.Expressions;

namespace Rent.Models.Projects
{
    public class FirestoreConversation
    {
        public int ID { get; set; }
        public int ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }
        public string ConversationID { get; set; }

        public static Expression<Func<FirestoreConversation, dynamic>> StandardDTO()
        {
            return v => v != null ?
            new
            {
                v.ConversationID,
                // project 
                //ProjectItem = ProjectItem.BasicDTO().Compile()(v.ProjectItem),
                ProjectItem_project = v.ProjectItem != null ? Project.BasicDTO().Compile()(v.ProjectItem.Project) : null,

            } : null;
        }
    }
}
