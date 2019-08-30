using System;
using System.ComponentModel.DataAnnotations;

namespace Rent.Models
{
    public class PermissionsTemplate
    {
		public int PermissionID { get; set; }
        public int RoleID { get; set; }
		public bool Create { get; set; }
		public bool Read { get; set; }
		public bool Update { get; set; }
		public bool Delete { get; set; }

        public virtual Permission Permission { get; set; }
	    
	    public class DTO : PermissionsTemplate
	    {
		    public string Name { get; set; }
		    
		    public DTO(PermissionsTemplate up)
		    {
			    SetBasics(up);
			    Name = up.Permission?.Name;
		    }
	    }

	    private void SetBasics(PermissionsTemplate up)
	    {
		    if(up == null)
			    return;
            
		    RoleID = up.RoleID;
		    PermissionID = up.PermissionID;
		    Create = up.Create;
		    Read = up.Read;
		    Update = up.Update;
		    Delete = up.Delete;
	    }
    }
	
	public static class TemplatePermissionDto
	{
		public static dynamic Standard(this PermissionsTemplate u)
		{
			if (u == null) return new { };
            
			return Merger.Merge(new { }, Basic(u));
		}
        
		public static dynamic Basic(this PermissionsTemplate p)
		{
			if (p == null) return new { };
			return new
			{
				p.Create,
				p.Read,
				p.Update,
				p.Delete,
				p.Permission?.Name,
				p.RoleID,
				p.PermissionID
			};
		}
	}
}
