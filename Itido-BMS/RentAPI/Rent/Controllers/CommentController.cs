using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rent.Models.Projects;
using Rent.Repositories;
namespace Rent.Controllers
{
    [Produces("application/json")]
    [Route("api/Comment")]
    public class CommentController : ControllerExecutor
    {
        private readonly CommentRepository _commentRepository;
        public CommentController(CommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        [HttpGet("Get/{commentId}")]
        public IActionResult Get(int commentId)
        => Executor(() => _commentRepository.Get(Requester, commentId));

        [HttpPut("Update/{commentId}")]
        public Task<IActionResult> Update(int commentId, [FromBody] Comment comment)
        => Executor(() => _commentRepository.Update(Requester, commentId, comment));
    }
}
