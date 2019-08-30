using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Exceptions;
using API.Models;
using API.Data;
using FirebaseAdmin.Auth;
using System.Linq;

namespace API.Controllers
{
    public class ControllerExecutor : Controller
    {
        // private readonly BMSContext _context;

        // public ControllerExecutor(BMSContext context)
        // {
        //     this._context = context;
        // }

        // public async Task<User> GetRequester()
        // {
        //     var token = Request.Headers["FirebaseToken"];
        //     var decoded = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
        //     var uid = decoded.Uid;
        //     var user = _context.Users.FirstOrDefault(u => u.FirebaseId == uid);
        //     return user;
        // }

        // async void test(String uid)
        // {
        //     var claims = new Dictionary<string, object>
        //     {
        //         { System.Security.Claims.ClaimTypes.Role, "Administrator" }
        //     };
        //     await FirebaseAuth.DefaultInstance.SetCustomUserClaimsAsync(uid, claims);
        // }



        protected async Task<IActionResult> Executor(Func<Task> func)
        {
            return await TryCatchAsync(async () =>
            {
                await func();
                return NoContent();
            });
        }

        protected IActionResult Executor(Action func)
        {
            return TryCatch(() =>
            {
                func();
                return NoContent();
            });
        }

        protected async Task<IActionResult> Executor<T>(Func<Task<T>> func)
        {
            return await TryCatchAsync(async () =>
            {
                var res = await func();
                if (res == null) throw new NotFoundException();
                return Ok(res);
            });
        }

        protected IActionResult Executor<T>(Func<T> func)
        {
            return TryCatch(() =>
            {
                var res = func();
                if (res == null) throw new NotFoundException();
                return Ok(res);
            });
        }
        // protected async Task<IActionResult> Executor(Func<User, Task> func)
        // {
        //     return await TryCatchAsync(async () =>
        //     {
        //         var requester = await GetRequester();
        //         await func(requester);
        //         return NoContent();
        //     });
        // }

        // protected async Task<IActionResult> Executor(Action<User> func)
        // {
        //     return await TryCatchAsync(async () =>
        //     {
        //         var requester = await GetRequester();
        //         func(requester);
        //         return NoContent();
        //     });
        // }

        // protected async Task<IActionResult> Executor<T>(Func<User, Task<T>> func)
        // {
        //     return await TryCatchAsync(async () =>
        //     {
        //         var requester = await GetRequester();
        //         var res = await func(requester);
        //         return Ok(res);
        //     });
        // }

        // protected async Task<IActionResult> Executor<T>(Func<User, T> func)
        // {
        //     return await TryCatchAsync(async () =>
        //     {
        //         var requester = await GetRequester();
        //         var res = func(requester);
        //         return Ok(res);
        //     });
        // }



        private IActionResult TryCatch(Func<IActionResult> func)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(ModelState);
                    return BadRequest(ModelState);
                }

                return func();
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                return Unauthorized();
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                return NotFound(e.Message);
            }
            catch (NothingUpdatedException e)
            {
                Console.WriteLine(e);
                return BadRequest("Nothing was updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }

        private async Task<IActionResult> TryCatchAsync(Func<Task<IActionResult>> func)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    Console.WriteLine(ModelState);
                    return BadRequest(ModelState);
                }

                return await func();
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
                Console.WriteLine(e.Message);
                return Unauthorized();
            }
            catch (NotFoundException e)
            {
                Console.WriteLine(e);
                return NotFound(e.Message);
            }
            catch (NothingUpdatedException e)
            {
                Console.WriteLine(e);
                return BadRequest("Nothing was updated");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }


        //public int Requester => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
