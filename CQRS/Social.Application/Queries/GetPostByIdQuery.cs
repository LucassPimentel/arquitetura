using MediatR;
using Social.Domain.Entities;

namespace Social.Application.Queries
{
    public class GetPostByIdQuery : IRequest<Post>
    {
        public Guid PostId { get; set; }
    }
}
