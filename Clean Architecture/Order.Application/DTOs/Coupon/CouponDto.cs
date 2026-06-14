namespace Order.Application.DTOs.Coupon
{
    public class CouponDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal Discount { get; set; }
        public decimal PercentualDiscount { get; set; }
        public decimal MaxDiscount { get; set; }
        public bool Active { get; set; }
        public DateTime ExpirationDate { get; set; }
    }
}
