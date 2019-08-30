using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Rent.Models.Projects;
using Rent.Models.General;

namespace Rent.Models
{
    public class DocumentFolder
    {
        public int ID { get; set; }
        public int? ParentDocumentFolderID { get; set; }
        public int? RootDocumentFolderID { get; set; }
        public string Title { get; set; }
        public bool Standard { get; set; }

        public bool VisibleToAllRoles { get; set; }

        public bool Removable { get; set; }

        public bool HasParentPermissions { get; set; }
        public ICollection<FolderPermission> FolderPermissions { get; set; }

        public int? ProjectItemID { get; set; }
        public virtual ProjectItem ProjectItem { get; set; }

        public ICollection<Document> Documents { get; set; }

        public virtual DocumentFolder RootDocumentFolder { get; set; }
        public virtual ICollection<DocumentFolder> DecendantDocumentFolders { get; set; }

        static public Expression<Func<DocumentFolder, dynamic>> BasicDTO()
        {
            return v => v != null ?
            new
            {
                v.ID,
                v.Title,
            } : null;
        }
    }
}
