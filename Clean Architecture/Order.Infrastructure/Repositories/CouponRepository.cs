using Microsoft.EntityFrameworkCore;
using Order.Domain;
using Order.Domain.Interfaces.Repositories;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.Repositories
{
    public class CouponRepository : ICouponRepository
    {
        private readonly AppDbContext _dbContext;

        public CouponRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Coupon coupon)
        {
            await _dbContext.Coupons.AddAsync(coupon);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Coupon?> GetByCodeAsync(string code)
        {
            return await _dbContext.Coupons
                .FirstOrDefaultAsync(c => c.Code == code.Trim().ToUpperInvariant());
        }

        public async Task<Coupon?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Coupons.FindAsync(id);
        }

        public async Task<IEnumerable<Coupon>> GetAllAsync()
        {
            return await _dbContext.Coupons.ToListAsync();
        }

        public void Update(Coupon coupon)
        {
            _dbContext.Coupons.Update(coupon);
            _dbContext.SaveChanges();
        }
    }
}
