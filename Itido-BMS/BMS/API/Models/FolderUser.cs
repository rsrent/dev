
namespace API.Models
{
    public class FolderUser
    {
        public long UserId { get; set; }
        public long FolderId { get; set; }
        public User User { get; set; }
        public Folder Folder { get; set; }
    }
}
