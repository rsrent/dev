using System.Collections.Generic;

namespace API.Models
{
    public class Organization
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string FirebaseOwnerId { get; set; }
        public ICollection<User> Users { get; set; }
    }
}
