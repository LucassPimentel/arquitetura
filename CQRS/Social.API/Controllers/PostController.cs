using MediatR;
using Microsoft.AspNetCore.Mvc;
using Social.Application.Commands;
using Social.Application.Queries;

namespace Social.API.Controllers
{
    [Route("api/v1/posts/")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IMediator _mediator;

        public PostController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostCommand command)
        {
            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetPostById), new { postId = result.Id }, result);
        }

        [HttpGet("user/{userId:guid}")]
        public async Task<IActionResult> GetPosts(Guid userId)
        {
            var query = new GetPostsByUserIdQuery { UserId = userId };
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> GetPostById(Guid postId)
        {
            var query = new GetPostByIdQuery { PostId = postId };
            var result = await _mediator.Send(query);
            return result == null ? NotFound() : Ok(result);
        }
    }
}
