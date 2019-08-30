using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using Rent.ContextPoint;
using Rent.Data;
using Rent.Models;

namespace Rent.Repositories
{
	public class PropCondition
    {
        private readonly RentContext _rentContext;
        
        private User _user;
        private List<UserPermissions> _userPermissions;
        private List<Permission> _permissions;
        private int _requester;

        public PropCondition(RentContext rentContext)
        {
            _rentContext = rentContext;
            _permissions = _rentContext.Permission.ToList();
        }

        public PropCondition SetUser(int requester) 
        {
            _requester = requester;
            _user = _rentContext.User.FirstOrDefault(u => u.ID == _requester);
            _userPermissions = _rentContext.UserPermissions.Where(up => up.UserID == requester).ToList();
            return this;
        }

        public int RoleID() => _user.RoleID;
        public int? CustomerID() => _user.CustomerID;
        
        public bool Unauthorized(string permission, CRUDD permissionType)
        {
            var per = _permissions.FirstOrDefault(p => p.Name == permission);
            var userPermission = _userPermissions.FirstOrDefault(up => up.PermissionID == per.ID);

            if (userPermission == null)
                return true;
            
            switch (permissionType)
            {
                case CRUDD.Create:
                    return !userPermission.Create;
                case CRUDD.Read:
                    return !userPermission.Read;
                case CRUDD.Update:
                    return !userPermission.Update;
                case CRUDD.Delete:
                    return !userPermission.Delete;
            }
            return true;
        }

        public T IsEmployee<T>(T val)
        {
            if (new[] { 1,2,3,4,5,6,7 }.Contains(RoleID())) return val;
            return default(T);
        }

        public T IsCustomer<T>(T val)
        {
            if (new[] { 8, 9 }.Contains(RoleID())) return val;
            return default(T);
        }

        public T IsKam<T>(T val)
        {
            if (new[] { 1, 2 }.Contains(RoleID())) return val;
            return default(T);
        }

        public T HasReadPermission<T>(string permission, T val)
        {
            if (!Unauthorized(permission, CRUDD.Read)) return val;
            return default(T);
        }
        
        
        
        
        public bool IsEmployee()
        {
            if (new[] { 1,2,3,4,5,6,7 }.Contains(RoleID())) return true;
            return false;
        }

        public bool IsCustomer()
        {
            if (new[] { 8, 9 }.Contains(RoleID())) return true;
            return false;
        }

        public bool IsKam()
        {
            if (new[] { 1, 2 }.Contains(RoleID())) return true;
            return false;
        }

        public bool HasReadPermission(string permission)
        {
            if (!Unauthorized(permission, CRUDD.Read)) return true;
            return false;
        }
    }
}
