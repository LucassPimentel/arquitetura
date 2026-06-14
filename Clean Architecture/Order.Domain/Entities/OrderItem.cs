using Order.Domain.Entities.Base;

namespace Order.Domain.Entities
{
    public sealed class OrderItem : Entity
    {
        public Product Product { get; private set; }
        public int Quantity { get; private set; }
        public decimal Price { get; private set; }
        public decimal SubTotal { get; private set; }

        private OrderItem()
        {
            Product = null!;
        }

        public OrderItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
            Price = CalculatePrice();
            SubTotal = CalculateSubTotal();
        }

        public decimal CalculateSubTotal()
        {
            return Quantity * Price;
        }

        public decimal CalculatePrice()
        {
            return Product.Price;
        }
    }
}
