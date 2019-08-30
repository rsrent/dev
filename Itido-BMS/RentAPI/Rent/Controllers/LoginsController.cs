using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Rent.Data;
using Rent.Models;
using Rent.DTOs;
using Rent.Repositories;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;


namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Logins")]
    public class LoginsController : ControllerExecutor
    {
        private readonly LoginRepository _loginRepository;

        public LoginsController(LoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        [HttpPost("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            return await Executor(async () =>
                await _loginRepository.Logout(User, Request.HttpContext.Connection.RemoteIpAddress.ToString()));
        }

        // POST: api/Logins/Login
        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> DoLogin([FromBody] Login login)
        {
            return await Executor(() => _loginRepository.Login(login));
        }

        // POST: api/Logins/Login
        [HttpPost("LoginWithToken")]
        //[AllowAnonymous]
        public async Task<IActionResult> DoLoginWithToken()
        {
            return await Executor(() => _loginRepository.LoginWithToken(Requester));
        }

        // POST: api/Logins/CreateLogin
        [HttpPost("CreateLogin/")]
        [HttpPost("CreateLogin/{customerId}")]
        public async Task<IActionResult> AddLogin([FromRoute] int? customerId, [FromBody] UserLoginCreateDTO userLoginCreateDto)
        {
            return await Executor(async () => await _loginRepository.AddLogin(Requester, userLoginCreateDto, customerId));
        }

        [HttpPost("CreateSystem/{userRole}/{projectRoleId}")]
        public async Task<IActionResult> CreateSystem([FromRoute] string userRole, [FromRoute] int projectRoleId, [FromBody] UserLoginCreateDTO userLoginCreateDto)
        {
            return await Executor(async () => await _loginRepository.CreateSystem(Requester, userLoginCreateDto, userRole, projectRoleId));
        }

        [HttpPost("CreateClient/{clientId}/{projectRoleId}")]
        public async Task<IActionResult> CreateClient([FromRoute] int clientId, [FromRoute] int projectRoleId, [FromBody] UserLoginCreateDTO userLoginCreateDto)
        {
            return await Executor(async () => await _loginRepository.CreateClient(Requester, userLoginCreateDto, clientId, projectRoleId));
        }

        [HttpPut("UpdatePassword")]
        public async Task<IActionResult> UpdateLogin([FromBody] UpdateLogin updateLogin)
        {
            return await Executor(async () => await _loginRepository.UpdatePassword(Requester, updateLogin));
        }

        [HttpPut("ForceUpdatePassword")]
        public async Task<IActionResult> ForceUpdatePassword([FromBody] UpdateLogin updateLogin)
        {
            return await Executor(async () => await _loginRepository.ForceUpdatePassword(Requester, updateLogin));
        }

        [HttpGet("GetUserUsername/{userId}")]
        public IActionResult GetUserUsername([FromRoute] int userId)
            => Executor(() => _loginRepository.GetUserUsername(Requester, userId));

        [HttpGet("HasValidToken")]
        public IActionResult HasValidToken()
        {
            return Ok(Int32.Parse(User.Claims.ToList()[0].Value));
        }


        static readonly Dictionary<string, List<string>> SupportedVersions = new Dictionary<string, List<string>>() {
            { "android", new List<string>() { "1.0.0" } },
            { "ios", new List<string>() { "1.0.0" } },
         };


        [AllowAnonymous]
        [HttpGet("ValidateVersion/{platform}/{version}")]
        public IActionResult HasValidToken([FromRoute] string platform, [FromRoute] string version)
        {
            var returnCode = 2;
            if (SupportedVersions[platform].First() == version) returnCode = 0;
            else if (SupportedVersions[platform].Contains(version)) returnCode = 1;

            return Ok(returnCode);
        }
    }
}