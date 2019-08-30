using System;
using Rent.Data;
using Rent.Models.TimePlanning;
using System.Linq;
using System.Collections.Generic;
using Rent.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Rent.Repositories.TimePlanning
{
    public class PostRepository
    {
        private readonly ProjectRoleRepository _projectRoleRepository;
        private readonly RentContext _context;
        private readonly IRoleAuthenticationRepository _roleAuthenticationRepository;
        private readonly NotiRepository _notiRepository;

        public PostRepository(ProjectRoleRepository projectRoleRepository, RentContext context, IRoleAuthenticationRepository roleAuthenticationRepository, NotiRepository notiRepository)
        {
            _projectRoleRepository = projectRoleRepository;
            this._context = context;
            this._roleAuthenticationRepository = roleAuthenticationRepository;
            _notiRepository = notiRepository;
        }

        /*

        public IQueryable<dynamic> GetLatest(int requester, int count)
        {
            var user = _context.User.Find(requester);
            if (user == null)
            {
                throw new NullReferenceException();
            }

            //Console.WriteLine("UserRole: " + )

            var posts = _context.Posts.Where(p =>
                user.UserRole.Equals("Admin") ||
                p.UserRole != null && p.UserRole.Equals(user.UserRole) ||
                p.LocationID != null && p.Location.LocationUsers.Any(lu => lu.UserID == user.ID) ||
                p.CustomerID != null && p.Customer.Locations.Any(l => l.LocationUsers.Any(lu => lu.UserID == user.ID))
                ).OrderByDescending(p => p.SendTime).Select(p => new
                {
                    p.ID,
                    p.Title,
                    p.Body,
                    p.SendTime,
                    Sender = new
                    {
                        p.User.ID,
                        p.User.FirstName,
                        p.User.LastName,
                        p.User.EmployeeNumber,
                    }
                }).Take(count);
            return posts;
        }

        public async Task<int> Create(int requester, Post post)
        {
            if (post == null)
            {
                throw new NullReferenceException();
            }
            if ((post.LocationID != null && post.CustomerID != null))
            {
                throw new UnauthorizedAccessException();
            }
            if (_roleAuthenticationRepository.IsAdmin(requester) || (_roleAuthenticationRepository.IsManager(requester) && post.UserRole == "User"))
            {
                post.UserId = requester;
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                await _notiRepository.SendPostNoti(requester, post);
                return post.ID;
            }
            throw new UnauthorizedAccessException();
        }

*/
    }
}
