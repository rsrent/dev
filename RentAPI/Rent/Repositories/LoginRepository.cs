using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Rent.Data;
using Rent.DTOs;
using Rent.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Rent.ContextPoint;
using Rent.ContextPoint.Exceptions;

namespace Rent.Repositories
{
    public class LoginRepository
    {
        private readonly RentContext _rentContext;

        private readonly LoginContext _loginContext;
        private readonly UserContext _userContext;
        private readonly PermissionRepository _permissionRepository;

        private readonly TokenRepository _tokenRepository;
        private readonly PropCondition _propCondition;

        public LoginRepository(RentContext rentContext, LoginContext loginContext, UserContext userContext, PermissionRepository permissionRepository, TokenRepository tokenRepository, PropCondition propCondition)
        {
            _rentContext = rentContext;
            _loginContext = loginContext;
            _userContext = userContext;
            _permissionRepository = permissionRepository;
            _tokenRepository = tokenRepository;
            _propCondition = propCondition;
        }
        
        
        public dynamic Login(Login login)
        {
            return Login(() =>
            {
                ValidateLogin(login);
                var loginId = GetLoginId(login);
                return u => u.LoginID == loginId;
            });
        }

        public string GetUserUsername(int requester, int userId)
        {
            var loginId = _userContext.DatabaseOne(requester, u => u.ID == userId).LoginID;
            return _loginContext.GetOne(requester, l => l.ID == loginId).UserName;
        }
        
        public dynamic LoginWithToken(int requester)
        {
            return Login(() => u => u.ID == requester);
        }
        
        private dynamic Login(Func<Expression<Func<User,bool>>> getUser)
        {
            var us = _userContext.DatabaseOne(0, getUser(), "Role").Detailed();
            
            return new
            {
                user = Merger.Merge(us, new { permissions = _permissionRepository.GetUserPermissions(0, us.ID)}),
                token = _tokenRepository.GenerateToken(us.ID.ToString())
            };
        }

        public async Task Logout(ClaimsPrincipal user, string ip)
        {
            var tokenGuid = user.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti)?.ToString();
            
            if(tokenGuid == null)
                throw new UnauthorizedAccessException();
            var userId = Int32.Parse(user.Claims.ToList()[0].Value);
            
            var loginId = _userContext.DatabaseOne(0, u => u.ID == userId).LoginID;
            
            
            
            await _rentContext.BlackListedToken.AddAsync(new BlackListedToken
            {
                TokenGuid = tokenGuid,
                BlackListTime = DateTime.Now,
                IP = ip,
                LoginID = loginId
            });
            await _rentContext.SaveChangesAsync();
        }
        
        
        private void ValidateLogin(Login login)
        {
            if (!VerifyHashedPassword(_loginContext.GetOne(0, l => l.UserName.Equals(login.UserName)).Password,
                login.Password))
            {
                throw new UnauthorizedAccessException("Wrong login");
            }
        }
        public int GetLoginId(Login login)
        {
            return _loginContext.GetOne(0, l => l.UserName.Equals(login.UserName)).ID;
        }

        public async Task<dynamic> AddLogin(int requester, UserLoginCreateDTO loginDTO)
        {
            //TODO: Implement localization on the return strings
            if (_rentContext.Login.Any(u => u.UserName.Equals(loginDTO.Login.UserName)))
                throw new UsernameTakenException();
            if (loginDTO.Login.Password.Length < 4)
                throw new UsernameTooShortException();

            var login = await _loginContext.Create(requester, new Login
            {
                UserName = loginDTO.Login.UserName,
                Password = HashPassword(loginDTO.Login.Password)
            });

            var user = loginDTO.User;
            user.LoginID = login.ID;
            user.Customer = null;

            var userDto = await _userContext.Create(requester, user);
            await _permissionRepository.CreateUserPermissions(requester, userDto);

            return userDto.Detailed();
        }

        public async Task<string> UpdatePassword(int requester, UpdateLogin updateLogin)
        {
            ValidateLogin(updateLogin.Login);
            return await ForceUpdatePassword(0, updateLogin);
        }

        public async Task<string> ForceUpdatePassword(int requester, UpdateLogin updateLogin)
        {
            var login = _loginContext.GetOne(0, l => l.UserName.Equals(updateLogin.Login.UserName));

            await _loginContext.Update(requester, l => l.ID == login.ID, l =>
            {
                l.Password = HashPassword(updateLogin.NewPassword);
                l.PasswordLastUpdated = DateTime.UtcNow;
            });
            var user = _userContext.DatabaseOne(0, u => u.LoginID == login.ID);
            return _tokenRepository.GenerateToken(user.ID.ToString());
        }

        public static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (var bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            var dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        private static bool VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                return false;
            }
            var src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            var dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            var buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (var bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return buffer3.SequenceEqual(buffer4);
        }
    }
}
