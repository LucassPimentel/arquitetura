using MediatR;
using Social.Domain.Entities;
using Social.Infrastructure.Persistence;

namespace Social.Application.Queries.QueryHandlers
{
    public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Post>
    {
        private readonly SocialDbContext _dbContext;

        public GetPostByIdHandler(SocialDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Post?> Handle(GetPostByIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Posts.FindAsync([request.PostId], cancellationToken);
        }
    }
}
