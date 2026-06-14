using Order.Domain.Entities.Base;
using Order.Domain.Errors;

namespace Order.Domain
{
    public sealed class Coupon : Entity
    {
        public string Code { get; private set; } = string.Empty;
        public string Name { get; private set; } = string.Empty;
        public decimal Discount { get; private set; }
        public decimal PercentualDiscount { get; private set; }
        public decimal MaxDiscount { get; private set; }
        public bool Active { get; private set; }
        public DateTime ExpirationDate { get; private set; }

        private Coupon() { }

        private Coupon(Guid id, string code, string name, decimal discount, decimal percentualDiscount, decimal maxDiscount, DateTime expirationDate)
        {
            Id = id;
            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Discount = discount;
            PercentualDiscount = percentualDiscount;
            MaxDiscount = maxDiscount;
            Active = true;
            ExpirationDate = expirationDate;
        }

        public static Result<Coupon> Create(string code, string name, decimal discount, decimal percentualDiscount, decimal maxDiscount, DateTime expirationDate)
        {
            var error = Validate(code, name, discount, percentualDiscount, maxDiscount, expirationDate);
            if (error is not null)
                return Result<Coupon>.Failure(error);

            return Result<Coupon>.Success(
                new Coupon(Guid.NewGuid(), code, name, discount, percentualDiscount, maxDiscount, expirationDate)
            );
        }

        public Result Update(string code, string name, decimal discount, decimal percentualDiscount, decimal maxDiscount, DateTime expirationDate)
        {
            var error = Validate(code, name, discount, percentualDiscount, maxDiscount, expirationDate);
            if (error is not null)
                return Result.Failure(error);

            Code = code.Trim().ToUpperInvariant();
            Name = name.Trim();
            Discount = discount;
            PercentualDiscount = percentualDiscount;
            MaxDiscount = maxDiscount;
            ExpirationDate = expirationDate;

            return Result.Success();
        }

        public Result Deactivate()
        {
            if (!Active)
                return Result.Failure(CouponErrors.AlreadyInactive);

            Active = false;
            return Result.Success();
        }

        public Result Activate()
        {
            if (Active)
                return Result.Failure(CouponErrors.AlreadyActive);

            Active = true;
            return Result.Success();
        }

        public Result IsValid()
        {
            if (!Active)
                return Result.Failure(CouponErrors.Inactive);

            if (DateTime.UtcNow > ExpirationDate)
                return Result.Failure(CouponErrors.Expired);

            return Result.Success();
        }

        private static Error? Validate(string code, string name, decimal discount, decimal percentualDiscount, decimal maxDiscount, DateTime expirationDate)
        {
            if (string.IsNullOrWhiteSpace(code))
                return CouponErrors.CodeRequired;

            if (code.Length > 20)
                return CouponErrors.CodeTooLong;

            if (string.IsNullOrWhiteSpace(name))
                return CouponErrors.NameRequired;

            if (name.Length > 100)
                return CouponErrors.NameTooLong;

            if (discount < 0)
                return CouponErrors.InvalidDiscount;

            if (percentualDiscount < 0 || percentualDiscount > 100)
                return CouponErrors.InvalidPercentualDiscount;

            if (discount == 0 && percentualDiscount == 0)
                return CouponErrors.NoDiscountDefined;

            if (maxDiscount <= 0)
                return CouponErrors.InvalidMaxDiscount;

            if (expirationDate <= DateTime.UtcNow)
                return CouponErrors.InvalidExpirationDate;

            return null;
        }
    }
}
