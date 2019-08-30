using System;
using System.Collections.Generic;

namespace Rent.Models
{
    public class DocumentFolder
    {
        public int ID { get; set; }
        public int? ParentDocumentFolderID { get; set; }
        public string Title { get; set; }
        public bool Standard { get; set; }

        public bool VisibleToAllRoles { get; set; }

		public bool Removable { get; set; }

        public bool HasParentPermissions { get; set; }
        public ICollection<FolderPermission> FolderPermissions { get; set; }
    }
}
