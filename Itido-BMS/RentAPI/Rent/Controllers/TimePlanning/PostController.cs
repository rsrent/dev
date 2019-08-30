using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.TimePlanning;
using Rent.Repositories.TimePlanning;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Rent.Controllers.TimePlanning
{
    [Produces("application/json")]
    [Route("api/Post")]
    public class PostController : ControllerExecutor
    {
        private readonly PostRepository _postRepository;
        public PostController(PostRepository postRepository)
        {
            _postRepository = postRepository;
        }
        /*
        [HttpGet("GetLatest/{count}")]
        public IActionResult GetLatest([FromRoute] int count)
        => Executor(() =>
        {
            var posts = _postRepository.GetLatest(Requester, count);
            return posts;
        });

        [HttpPost("Create")]
        public Task<IActionResult> Create([FromBody] Post post)
        => Executor(async () => await _postRepository.Create(Requester, post));
        */
    }
}
