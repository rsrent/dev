using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.Repositories;

namespace Rent.ContextPoint
{
    public class LoginContext : Context<Login>
    {
        public LoginContext(RentContext rentContext) : base(rentContext) { }

        protected override string Permission() => "Login";

        protected override DbSet<Login> GetDb() => RentContext.Login;
    }
}