using Order.Application.DTOs.Coupon;
using Order.Domain.Errors;

namespace Order.Application.Interfaces
{
    public interface ICouponService
    {
        Task<Result<CouponDto>> CreateAsync(CreateCouponDto dto);
        Task<Result<CouponDto>> UpdateAsync(UpdateCouponDto dto);
        Task<Result> DeactivateAsync(Guid id);
        Task<Result> ActivateAsync(Guid id);
        Task<IEnumerable<CouponDto>> GetAllAsync();
    }
}
