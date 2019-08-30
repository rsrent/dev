using System;
using Rent.Repositories;

namespace Rent.Models
{
    public abstract class IDto// : ProtectedObject
    {
        public virtual dynamic Detailed()
        {
            return Basic();
        }
        public abstract dynamic Basic();
    }
}
