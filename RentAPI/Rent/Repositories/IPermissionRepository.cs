using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Rent.Models;

namespace Rent
{
    public interface IPermissionRepository
    {
        ICollection<Permission> GetPermissions(int requester);
        ICollection<UserPermissions> GetUserPermissions(int requester, int userId);
        ICollection<PermissionsTemplate> GetTemplatePermissions(int requester, int roleId);
        Task CreatePermission(int requester, Permission permission);
        bool Unauthorized(ClaimsPrincipal user, string permission, CRUDD permissionType);
        bool Unauthorized(int userId, string permission, CRUDD permissionType);
        Task ResetUserPermissions(int requester, int userId);
        Task ResetUsersSpecificPermissions(int requester, int permissionId);
        Task UpdateUserPermission(int requester, int userId, int permissionId, CRUDD crudd, bool active);
        Task UpdateTemplatePermission(int requester, int roleId, int permissionId, CRUDD crudd, bool active);
        Task<ICollection<UserPermissions>> CreateUserPermissions(int requester, User user);
    }
}