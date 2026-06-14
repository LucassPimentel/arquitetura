using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.Infrastructure.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products => Set<Product>();
        public DbSet<Domain.Entities.Order> Orders => Set<Domain.Entities.Order>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Coupon> Coupons => Set<Coupon>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
