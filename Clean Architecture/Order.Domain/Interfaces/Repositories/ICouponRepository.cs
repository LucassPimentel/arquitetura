namespace Order.Domain.Interfaces.Repositories
{
    public interface ICouponRepository
    {
        Task<Coupon?> GetByIdAsync(Guid id);
        Task<Coupon?> GetByCodeAsync(string code);
        Task<IEnumerable<Coupon>> GetAllAsync();
        Task AddAsync(Coupon coupon);
        void Update(Coupon coupon);
    }
}
