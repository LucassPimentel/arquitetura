using MediatR;
using Social.Domain.Entities;
using Social.Infrastructure.Persistence;

namespace Social.Application.Commands.CommandHandlers
{
    public class CreatePostCommandHandler : IRequestHandler<CreatePostCommand, Post>
    {
        private readonly SocialDbContext _context;

        public CreatePostCommandHandler(SocialDbContext context)
        {
            _context = context;
        }

        public async Task<Post> Handle(CreatePostCommand request, CancellationToken cancellationToken)
        {
            var post = Post.PostMessage(request.UserId, request.Message);
            await _context.AddAsync(post);
            await _context.SaveChangesAsync();

            return post;
        }
    }
}
