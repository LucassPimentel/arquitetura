using EWallet.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EWallet.Infrastructure.Context
{
    public class EWalletDbContext : DbContext
    {
        public EWalletDbContext(DbContextOptions<EWalletDbContext> options) : base(options)
        {
        }

        public DbSet<EventRecord> Events => Set<EventRecord>();
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<EventRecord>(entity =>
            {
                entity.ToTable("EventStore");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EventData).IsRequired();
                entity.HasIndex(e => new { e.EntityId, e.Version }).IsUnique(true);
            });
        }
    }
}
