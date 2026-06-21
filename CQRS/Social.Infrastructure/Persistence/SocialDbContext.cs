using Microsoft.EntityFrameworkCore;
using Social.Domain.Entities;

namespace Social.Infrastructure.Persistence
{
    public class SocialDbContext : DbContext
    {
        public SocialDbContext(DbContextOptions<SocialDbContext> options) : base(options)
        {
        }

        public DbSet<Post> Posts => Set<Post>();
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
