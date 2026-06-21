using MediatR;
using Social.Domain.Entities;

namespace Social.Application.Commands
{
    public class CreatePostCommand : IRequest<Post>
    {
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }
}
