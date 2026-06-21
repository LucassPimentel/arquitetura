using MediatR;
using Microsoft.EntityFrameworkCore;
using Social.Domain.Entities;
using Social.Infrastructure.Persistence;

namespace Social.Application.Queries.QueryHandlers
{
    public class GetPostsByUserIdQueryHandler : IRequestHandler<GetPostsByUserIdQuery, User>
    {
        private readonly SocialDbContext _dbContext;
        public GetPostsByUserIdQueryHandler(SocialDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User?> Handle(GetPostsByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _dbContext.Users
                .Include(u => u.Posts)
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
        }
    }
}
