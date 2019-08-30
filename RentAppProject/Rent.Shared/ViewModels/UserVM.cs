using System;
using ModuleLibrary.Shared.Storage;
using RentAppProject;
using static RentAppProject.Permission;

namespace Rent.Shared.ViewModels
{
    public class UserVM
    {
        private static User User { get; set; }

        public bool HasPermission(string permissionName, CRUDD crudd)
        {
            return Permission.HasPermission(User, permissionName, crudd);
        }

        public User LoggedInUser() => User;

        public int ID => User.ID;

        public void SetLoggedInUser(User user) {
            User = user;
        }
    }
}
