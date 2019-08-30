using Rent.Data;
using Rent.Models;
using System.Linq;
using System;

namespace Rent.Repositories.TimePlanning
{
    public class RoleAuthenticationRepository : IRoleAuthenticationRepository
    {


        private readonly RentContext _rentContext;
        public RoleAuthenticationRepository(RentContext rentContext)
        {
            _rentContext = rentContext;
        }

        public bool IsAdmin(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            if (user != null && user.UserRole.Equals("Admin"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsAdminOrManager(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            Console.WriteLine("Is admin or manager called");
            Console.WriteLine(user);
            if (user != null && (user.UserRole.Equals("Manager") || user.UserRole.Equals("Admin")))
            {
                Console.WriteLine("Is admin or manager called and return true");

                return true;
            }
            else
            {
                Console.WriteLine("Is admin or manager called and return false");

                return false;
            }
        }
        public bool IsManager(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            if (user != null && (user.UserRole.Equals("Manager")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        public bool IsUser(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            if (user != null && (user.UserRole.Equals("User")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsClientAdmin(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            if (user != null && (user.UserRole.Equals("ClientAdmin")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsClientManager(int requester)
        {
            if (requester == 0) return true;
            var user = _rentContext.User.Find(requester);
            if (user != null && (user.UserRole.Equals("ClientManager")))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public string GetRole(int requester)
        {
            if (requester == 0) return "Admin";
            var user = _rentContext.User.Find(requester);
            if (user != null)
            {
                return user.UserRole;
            }
            else
            {
                return null;
            }
        }

        public bool IsClientAdminOrClientManager(int requester) => IsClientAdmin(requester) || IsClientManager(requester);
    }

}