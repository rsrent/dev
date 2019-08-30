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
            return Executor(() => _loginRepository.Login(login));
        }

        // POST: api/Logins/Login
        [HttpPost("LoginWithToken")]
        [AllowAnonymous]
        public IActionResult DoLoginWithToken()
        {
            return Executor(() => _loginRepository.LoginWithToken(Requester));
        }

        // POST: api/Logins/CreateLogin
        [HttpPost("CreateLogin")]
        public async Task<IActionResult> AddLogin([FromBody] UserLoginCreateDTO userLoginCreateDto)
        {
            return await Executor(async () => await _loginRepository.AddLogin(Requester, userLoginCreateDto));
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
    }
}