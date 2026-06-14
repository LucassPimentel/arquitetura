using Order.Domain.Entities.Base;
using Order.Domain.Enums;
using Order.Domain.Errors;

namespace Order.Domain.Entities
{
    public sealed class Order : Entity
    {
        public int ClientId { get; private set; }
        public Coupon? Coupon { get; private set; }
        public decimal TotalPrice { get; private set; }
        public decimal FinalPrice { get; private set; }
        public Status Status { get; private set; }
        public List<OrderItem> Items { get; private set; } = new List<OrderItem>();
        public DateTime CreatedAt { get; private set; }
        public DateTime ModifiedAt { get; private set; }

        private Order() { }

        private Order(Guid id, int clientId, Coupon? coupon, List<OrderItem> items)
        {
            Id = id;
            ClientId = clientId;
            Coupon = coupon;
            Status = Status.Open;
            CreatedAt = DateTime.UtcNow;
            ModifiedAt = DateTime.UtcNow;
            Items = items;
            RecalculatePrices();
        }

        public static Result<Order> Create(int clientId, List<OrderItem> items)
        {
            var error = Validate(clientId, items);
            if (error is not null)
                return Result<Order>.Failure(error);

            return Result<Order>.Success(
                new Order(Guid.NewGuid(), clientId, null, items)
            );
        }

        public Result Update(List<OrderItem> items)
        {
            var error = Validate(ClientId, items);
            if (error is not null)
                return Result.Failure(error);

            Items = items;
            RecalculatePrices();
            ModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result AddItem(OrderItem item)
        {
            var statusValidationResult = ValidateOrderStatus(Status);
            if (!statusValidationResult.IsSuccess)
                return statusValidationResult;

            Items.Add(item);
            RecalculatePrices();
            ModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result RemoveItem(Guid itemId)
        {
            var statusValidationResult = ValidateOrderStatus(Status);
            if (!statusValidationResult.IsSuccess)
                return statusValidationResult;

            if (Items.Count <= 1)
                return Result.Failure(OrderErrors.EmptyOrder);

            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item is null)
                return Result.Failure(OrderErrors.ItemNotFound);

            Items.Remove(item);
            RecalculatePrices();
            ModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public void RecalculatePrices()
        {
            TotalPrice = Items.Sum(x => x.SubTotal);
            FinalPrice = CalculateFinalPrice();
            ModifiedAt = DateTime.UtcNow;
        }

        public Result ApplyCoupon(Coupon coupon)
        {
            var statusValidationResult = ValidateOrderStatus(Status);
            if (!statusValidationResult.IsSuccess)
            {
                return statusValidationResult;
            }

            if (Coupon != null)
            {
                return Result.Failure(OrderErrors.CouponAlreadyApplied);
            }

            var couponValidationResult = coupon.IsValid();
            if (!couponValidationResult.IsSuccess)
            {
                return couponValidationResult;
            }

            Coupon = coupon;
            RecalculatePrices();
            ModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public Result CloseOrder()
        {
            var statusValidationResult = ValidateOrderStatus(Status);
            if (!statusValidationResult.IsSuccess)
            {
                return statusValidationResult;
            }

            if (Items.Count < 1)
            {
                return Result.Failure(OrderErrors.EmptyOrder);
            }

            if (TotalPrice <= 0 || FinalPrice <= 0)
            {
                return Result.Failure(OrderErrors.InvalidPrice);
            }

            Status = Status.Finished;
            ModifiedAt = DateTime.UtcNow;
            return Result.Success();
        }

        public void CancelOrder()
        {
            Status = Status.Canceled;
            ModifiedAt = DateTime.UtcNow;
        }

        private static Error? Validate(int clientId, List<OrderItem> items)
        {
            if (clientId <= 0)
                return OrderErrors.InvalidClientId;

            if (items == null || items.Count == 0)
                return OrderErrors.EmptyOrder;

            return null;
        }

        private Result ValidateOrderStatus(Status status)
        {
            switch (status)
            {
                case Status.Canceled:
                    return Result.Failure(OrderErrors.Canceled);
                case Status.Finished:
                    return Result.Failure(OrderErrors.Finished);
            }
            return Result.Success();
        }

        private decimal CalculateFinalPrice()
        {
            if (Coupon == null)
                return TotalPrice;

            decimal discountAmount = Coupon.Discount + (TotalPrice * Coupon.PercentualDiscount / 100);
            if (discountAmount > Coupon.MaxDiscount)
                discountAmount = Coupon.MaxDiscount;

            return TotalPrice - discountAmount;
        }
    }
}
