using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.ContextPoint.Exceptions;
using Rent.Models;

namespace Rent.Controllers
{
    public class ControllerExecutor : Controller
    {

        object CheckIfLegal (object t)
        {
            if(t is ProtectedObject)
            {
                throw new ProtectedObjectException();
            }

            if (t is IEnumerable<ProtectedObject>)
            {
                throw new ProtectedObjectException();
            }

            return t;
        }

        
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
            return await TryCatchAsync(async () => Ok(CheckIfLegal(await func())));
        }

        protected IActionResult Executor<T>(Func<T> func)
        {
            return TryCatch(() => Ok(CheckIfLegal(func())));
        }
        
        
        
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
            catch (ProtectedObjectException e)
            {
                Console.WriteLine(e);
                return BadRequest("Returned ProtectedObject");
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
            catch (ProtectedObjectException e)
            {
                Console.WriteLine(e);
                return BadRequest("Returned ProtectedObject");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500);
            }
        }
        

        public int Requester => Int32.Parse(User.Claims.ToList()[0].Value);
    }
}
