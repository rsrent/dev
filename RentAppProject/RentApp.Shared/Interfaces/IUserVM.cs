using System;
using RentAppProject;
using static RentAppProject.Permission;

namespace RentApp
{
    public interface IUserVM
    {
        User LoggedInUser();
        int UserID();
        bool IsUserID(int id);
        void SetLoggedInUser(User user);
        bool HasPermission(string permissionName, CRUDD crudd);
    }
}
