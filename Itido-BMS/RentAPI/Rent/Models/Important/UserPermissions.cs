using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Models
{
    public class UserPermissions
    {
        public int UserID { get; set; }
        public int PermissionID { get; set; }
        public bool Create { get; set; }
        public bool Read { get; set; }
        public bool Update { get; set; }
        public bool Delete { get; set; }

        public virtual User User { get; set; }
        public virtual Permission Permission { get; set; }        
    }
    
    public static class UserPermissionDto
    {
        public static dynamic Standard(this UserPermissions u)
        {
            if (u == null) return new { };
            
            return Merger.Merge(new { }, Basic(u));
        }
        
        public static dynamic Basic(this UserPermissions p)
        {
            if (p == null) return new { };
            return new
            {
                p.Create,
                p.Read,
                p.Update,
                p.Delete,
                p.Permission?.Name,
                p.UserID,
                p.PermissionID
            };
        }
    }
}
