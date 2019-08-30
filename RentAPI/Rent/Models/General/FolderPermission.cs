using System;
namespace Rent.Models
{
    public class FolderPermission
    {
        public int FolderID { get; set; }
        public int RoleID { get; set; }
        public bool Read { get; set; }
        public bool Edit { get; set; }

        public virtual DocumentFolder Folder { get; set; }
    }
}
