using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Models;

namespace API.Repositories
{
    public interface IUserRepository
    {
        Task<User> Get(long Id);
        Task<ICollection<User>> GetMany();
        Task<User> Create(User user);
        Task Update(long id, User user);
        Task Delete(long id);
    }
}
