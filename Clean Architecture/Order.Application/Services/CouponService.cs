using AutoMapper;
using Order.Application.DTOs.Coupon;
using Order.Application.Interfaces;
using Order.Domain;
using Order.Domain.Errors;
using Order.Domain.Interfaces.Repositories;

namespace Order.Application.Services
{
    public class CouponService : ICouponService
    {
        private readonly ICouponRepository _repository;
        private readonly IMapper _mapper;

        public CouponService(ICouponRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<CouponDto>> CreateAsync(CreateCouponDto dto)
        {
            var existing = await _repository.GetByCodeAsync(dto.Code);
            if (existing is not null)
                return Result<CouponDto>.Failure(CouponErrors.AlreadyExists);

            var result = Coupon.Create(
                dto.Code,
                dto.Name,
                dto.Discount,
                dto.PercentualDiscount,
                dto.MaxDiscount,
                dto.ExpirationDate
            );

            if (!result.IsSuccess)
                return Result<CouponDto>.Failure(result.Error!);

            await _repository.AddAsync(result.Value!);
            return Result<CouponDto>.Success(_mapper.Map<CouponDto>(result.Value!));
        }

        public async Task<Result<CouponDto>> UpdateAsync(UpdateCouponDto dto)
        {
            var coupon = await _repository.GetByIdAsync(dto.Id);
            if (coupon is null)
                return Result<CouponDto>.Failure(CouponErrors.NonExistent);

            var result = coupon.Update(
                dto.Code,
                dto.Name,
                dto.Discount,
                dto.PercentualDiscount,
                dto.MaxDiscount,
                dto.ExpirationDate
            );

            if (!result.IsSuccess)
                return Result<CouponDto>.Failure(result.Error!);

            _repository.Update(coupon);
            return Result<CouponDto>.Success(_mapper.Map<CouponDto>(coupon));
        }

        public async Task<Result> DeactivateAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            if (coupon is null)
                return Result.Failure(CouponErrors.NonExistent);

            var result = coupon.Deactivate();
            if (!result.IsSuccess)
                return result;

            _repository.Update(coupon);
            return Result.Success();
        }

        public async Task<Result> ActivateAsync(Guid id)
        {
            var coupon = await _repository.GetByIdAsync(id);
            if (coupon is null)
                return Result.Failure(CouponErrors.NonExistent);

            var result = coupon.Activate();
            if (!result.IsSuccess)
                return result;

            _repository.Update(coupon);
            return Result.Success();
        }

        public async Task<IEnumerable<CouponDto>> GetAllAsync()
        {
            var coupons = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<CouponDto>>(coupons);
        }
    }
}
