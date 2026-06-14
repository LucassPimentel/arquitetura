using Order.Domain.Enums;

namespace Order.Application.DTOs.Order
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public int ClientId { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal DiscountAmount => TotalPrice - FinalPrice;
        public string? CouponCode { get; set; }
        public Status Status { get; set; }
        public List<OrderItemDto> Items { get; set; } = new();
        public DateTime CreatedAt { get; set; }
        public DateTime ModifiedAt { get; set; }
    }

    public class OrderItemDto
    {
        public Guid Id { get; set; }
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
    }
}
