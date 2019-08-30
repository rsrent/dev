using System;
using System.Collections.Generic;

namespace API.Models
{
    public class User
    {
        public long Id { get; set; }
        public long OrganizationId { get; set; }
        public string FirebaseId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Role { get; set; }
        public virtual ICollection<UnitUser> UnitUsers { get; set; }
        public virtual ICollection<ProjectUser> ProjectUsers { get; set; }

        public Organization Organization { get; set; }
    }
}
