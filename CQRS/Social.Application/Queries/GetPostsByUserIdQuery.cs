using MediatR;
using Social.Domain.Entities;

namespace Social.Application.Queries
{
    public class GetPostsByUserIdQuery : IRequest<User>
    {
        public Guid UserId { get; set; }
    }
}
