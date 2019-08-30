using System.Collections.Generic;

namespace API.Models
{
    public class Folder
    {
        public long Id { get; set; }
        public long ParentId { get; set; }
        public Folder Parent { get; set; }
        public virtual ICollection<FolderUser> FolderUsers { get; set; }
    }
}
