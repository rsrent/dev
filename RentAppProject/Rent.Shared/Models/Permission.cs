using System;
using System.Linq;

namespace RentAppProject
{
    public class Permission
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public static string TEAM => PermissionNames[0];
        public static string USER => PermissionNames[1];
        public static string CUSTOMER => PermissionNames[2];
        public static string LOCATION => PermissionNames[3];
        public static string CleaningPlan => PermissionNames[4];
        public static string QualityReport => PermissionNames[5];
        public static string CompletedTask => PermissionNames[6];
        public static string LOGIN => PermissionNames[7];
        public static string PERMISSION => PermissionNames[8];
        public static string REGULARTASK => PermissionNames[9];
        public static string WINDOWTASK => PermissionNames[10];
        public static string FANCOILTASK => PermissionNames[11];
        public static string CHAT => PermissionNames[12];
        public static string DOCUMENT => PermissionNames[13];
        public static string ECONOMY => PermissionNames[14];
        public static string HOUR => PermissionNames[15];

		public static string[] PermissionNames = new[] {
			"Team",
			"User",
			"Customer",
			"Location",
            "CleaningTask",
			"QualityReport",
			"CompletedTask",
			"Login",
			"Permission",

            "RegularTask",
            "WindowTask",
            "FanCoilTask",

            "Chat",
            "Document",
            "Economy",
            "Hour",
		};

        public static bool HasPermission(User user, string permissionName, CRUDD crudd) {
            if (user == null || user.Permissions == null || !user.Permissions.Any(s => s.Name.Equals(permissionName)))
				return false;

            var permission = user.Permissions.FirstOrDefault(p => p.Name == permissionName);

            switch (crudd) {
                case CRUDD.Create:
                    return permission.Create;
                case CRUDD.Read:
                    return permission.Read;
                case CRUDD.Update:
                    return permission.Update;
                case CRUDD.Delete:
                    return permission.Delete;
            }
            return false;
        }

        public enum CRUDD {
            Create, Read, Update, Delete
        }
    }
}
