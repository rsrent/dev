using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rent.Repositories
{
    public interface ITokenRepository
    {
        string GenerateToken(string userID);
    }
}
