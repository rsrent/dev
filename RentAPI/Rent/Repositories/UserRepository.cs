using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Rent.ContextPoint;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;

namespace Rent.Repositories
{
    public class UserRepository
    {
        private readonly RentContext _rentContext;
        private readonly UserContext _userContext;
        private readonly LocationContext _locationContext;
        private readonly LocationUserContext _locationUserContext;
        private readonly PropCondition _propCondition;

        public UserRepository(RentContext rentContext, UserContext userContext, LocationContext locationContext, LocationUserContext locationUserContext, PropCondition propCondition)
        {
            _rentContext = rentContext;
            _userContext = userContext;
            _locationContext = locationContext;
            _locationUserContext = locationUserContext;
            _propCondition = propCondition;
        }

        public object Get(int requester, int userId)
        {
            return _userContext.DetailedOne(requester, (u) => u.ID == userId, "Role");
        }

        public IEnumerable<dynamic> GetAll(int requester)
        {
            return _userContext.Basic(requester, null, "Role", "Customer");
        }

        public IEnumerable<dynamic> GetWithRole(int requester, int roleId)
        {
            return _userContext.Basic(requester, (u) => u.RoleID == roleId, "Role");
        }

        public IEnumerable<dynamic> GetLocationUsers(int requester, int locationId)
        {
            return _locationUserContext.Get(requester, (lu) => lu.LocationID == locationId, "User.Role").Select(lu => lu.ToUser()).ToList().OrderBy(u => u.RoleID);
        }

        public IEnumerable<dynamic> GetCustomerUsers(int requester, int customerId)
        {
            return _userContext.Basic(requester, (u) => u.CustomerID == customerId, "Role");
        }

        public IEnumerable<dynamic> GetPotentialUsersForLocation(int requester, int locationId)
        {
            var location = _locationContext
                .DatabaseOne(requester, (l) => l.ID == locationId);
            var locationUsers = _locationUserContext
                .Get(requester, (lu) => lu.LocationID == locationId).Select(lu => lu.User.ID);

            return _userContext.Basic(requester, (u) => !locationUsers.Contains(u.ID) && ((u.CustomerID == location.CustomerID) || (u.CustomerID == null)), "Role");
        }

        public async Task UpdateUser(int requester, User updatedUser)
        {
            await _userContext.Update(requester, u => u.ID == updatedUser.ID, u =>
            {
                u.FirstName = updatedUser.FirstName;
                u.LastName = updatedUser.LastName;
                u.Email = updatedUser.Email;
                u.Comment = updatedUser.Comment;
                u.ImageLocation = updatedUser.ImageLocation;
                u.EmployeeNumber = updatedUser.EmployeeNumber;
                u.Title = updatedUser.Title;
            });
        }

        public async Task UpdateUserImage(int requester, int userId, string userImage)
        {
            await _userContext.Update(requester, u => u.ID == userId, u =>
            {
                u.ImageLocation = userImage;
            });
        }

        public async Task Disable(int requester, int userId)
        {
            await _userContext.Update(requester, u => u.ID == userId, u =>
            {
                u.Disabled = true;
            });
        }

        public async Task Enable(int requester, int userId)
        {
            await _userContext.Update(requester, u => u.ID == userId, u =>
            {
                u.Disabled = false;
            });
        }
    }
}
