using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using Rent.Repositories;

namespace Rent
{
    public class PermissionRepository //: IPermissionRepository
        //: IPermissionRepository
    {
        //private readonly RentContext _context;

        private readonly UserPermissionContext _userPermissionContext;
        private readonly TemplatePermissionContext _templatePermissionContext;
        private readonly UserContext _userContext;
        private readonly RoleContext _roleContext;
        private readonly PermissionContext _permissionContext;
        
        public PermissionRepository(RentContext context, UserPermissionContext userPermissionContext, TemplatePermissionContext templatePermissionContext, PermissionContext permissionContext, UserContext userContext, RoleContext roleContext)
        {
            //_context = context;

            _userPermissionContext = userPermissionContext;
            _templatePermissionContext = templatePermissionContext;
            _permissionContext = permissionContext;
            _userContext = userContext;
            _roleContext = roleContext;
        }

        public IEnumerable<Permission> GetPermissions(int requester)
        {
            return _permissionContext.Get(requester, null);
        }
        
        public IEnumerable<dynamic> GetUserPermissions(int requester, int userId)
        {
            return _userPermissionContext.Get(requester, up => up.UserID == userId, "Permission").Select(p => p.Standard());
        }
        
        public IEnumerable<dynamic> GetTemplatePermissions(int requester, int roleId)
        {
            return _templatePermissionContext.Get(requester, tp => tp.RoleID == roleId, "Permission").Select(p => p.Standard());
        }

        public async Task CreatePermission(int requester, Permission permission)
        {
            await _permissionContext.Create(requester, permission);
            var templatePermissions = _roleContext.Get(requester, null).Select(r => new PermissionsTemplate
            {
                PermissionID = permission.ID,
                RoleID = r.ID,
                Create = false,
                Update = false,
                Delete = false,
                Read = false
            }).ToList();
            await _templatePermissionContext.Create(requester, templatePermissions);

            var userPermissions = _userContext.Database(requester, null).Select(u => new UserPermissions
            {
                PermissionID = permission.ID,
                UserID = u.ID,
                Create = false,
                Update = false,
                Delete = false,
                Read = false
            }).ToList();
            await _userPermissionContext.Create(requester, userPermissions);
        }

        public bool Unauthorized(ClaimsPrincipal user, string permission, CRUDD permissionType)
        {
            return Unauthorized(Int32.Parse(user.Claims.ToList()[0].Value), permission, permissionType);
        }

        public bool Unauthorized(int userId, string permission, CRUDD permissionType)
        {
            var userPermissions = _userPermissionContext.GetOne(0,
                up => up.UserID == userId && up.Permission.Name.Equals(permission), "Permission");
            
            switch (permissionType)
            {
                case CRUDD.Create:
                    return !userPermissions.Create;
                case CRUDD.Read:
                    return !userPermissions.Read;
                case CRUDD.Update:
                    return !userPermissions.Update;
                case CRUDD.Delete:
                    return !userPermissions.Delete;
            }
            return true;
        }

        public async Task ResetUserPermissions(int requester, int userId)
        {
            var user = _userContext.DatabaseOne(requester, u => u.ID == userId);

            var templatePermissions = _templatePermissionContext.Get(requester, tp => tp.RoleID == user.RoleID);
            
            await _userPermissionContext.Update(requester, up => up.UserID == userId, up =>
            {
                var tp = templatePermissions.FirstOrDefault(t => t.PermissionID == up.PermissionID);
                if (tp == null) return;
                up.Create = tp.Create;
                up.Read = tp.Read;
                up.Update = tp.Update;
                up.Delete = tp.Delete;
            });
        }

        public async Task ResetUsersSpecificPermissions(int requester, int permissionId)
        {
            //var permission = _permissionContext.GetOne(requester, p => p.ID == permissionId);
            var templatePermissions = _templatePermissionContext.Get(requester, tp => tp.PermissionID == permissionId);

            await _userPermissionContext.Update(requester, up => up.PermissionID == permissionId, up =>
            {
                var templatePermission = templatePermissions.FirstOrDefault(t => t.RoleID == up.User.RoleID);
                if (templatePermission == null) return;
                up.Create = templatePermission.Create;
                up.Read = templatePermission.Read;
                up.Update = templatePermission.Update;
                up.Delete = templatePermission.Delete;
            }, "User");
        }

        public async Task UpdateUserPermission(int requester, int userId, int permissionId, CRUDD crudd, bool active)
        {
            await _userPermissionContext.Update(requester, up => up.PermissionID == permissionId && up.UserID == userId,
                up =>
                {
                    switch (crudd)
                    {
                        case CRUDD.Create:
                            up.Create = active;
                            break;
                        case CRUDD.Read:
                            up.Read = active;
                            break;
                        case CRUDD.Update:
                            up.Update = active;
                            break;
                        case CRUDD.Delete:
                            up.Delete = active;
                            break;
                    }
                });
        }
        
        public async Task UpdateTemplatePermission(int requester, int roleId, int permissionId, CRUDD crudd, bool active)
        {
            await _templatePermissionContext.Update(requester, tp => tp.PermissionID == permissionId && tp.RoleID == roleId,
                tp =>
                {
                    switch (crudd)
                    {
                        case CRUDD.Create:
                            tp.Create = active;
                            break;
                        case CRUDD.Read:
                            tp.Read = active;
                            break;
                        case CRUDD.Update:
                            tp.Update = active;
                            break;
                        case CRUDD.Delete:
                            tp.Delete = active;
                            break;
                    }
                });
        }

        public async Task<IEnumerable<UserPermissions>> CreateUserPermissions(int requester, User user)
        {
            var newUserPermissions = _templatePermissionContext.Get(requester, tp => tp.RoleID == user.RoleID).Select(tp =>
                new UserPermissions
                {
                    UserID = user.ID,
                    PermissionID = tp.PermissionID,
                    Create = tp.Create,
                    Read = tp.Read,
                    Update = tp.Update,
                    Delete = tp.Delete
                }).ToList();

            return await _userPermissionContext.Create(requester, newUserPermissions);
        }
    }
}
