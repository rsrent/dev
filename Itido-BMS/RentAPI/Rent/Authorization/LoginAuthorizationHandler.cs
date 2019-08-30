using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Rent.Data;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace Rent.Authorization
{
    public class LoginAuthorizationHandler : AuthorizationHandler<AuthRequirement>
    {
        private readonly IServiceScopeFactory scopeFactory;

        public LoginAuthorizationHandler(IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthRequirement requirement)
        {
            using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<RentContext>();
                var claims = context.User.Claims;
                if(claims.ToList().Count == 0) // If the claims are empty, the token must be invalid. In this case due to a changing JWT Key
                {
                    context.Fail();
                    return Task.CompletedTask;
                }

                System.Diagnostics.Debug.WriteLine(claims);

                var userID = Int32.Parse(claims.ToList()[0].Value);

                var issuedAt = Int32.Parse(claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Iat)?.Value);

                var tokenGuid = claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.ToString();

                var user = dbContext.User.Find(userID);

                var login = dbContext.Login.Find(user.LoginID);

                var blackListed = dbContext.BlackListedToken.Any(bl => bl.TokenGuid.Equals(tokenGuid));

                if (new DateTimeOffset(login.PasswordLastUpdated).ToUnixTimeSeconds() <= issuedAt && !blackListed && !user.Disabled)
                {
                    context.Succeed(requirement);
                }

                return Task.CompletedTask;
                //throw new NotImplementedException();
            }
        }
    }
}
