using Microsoft.Extensions.DependencyInjection;
using Order.Domain;
using Order.Domain.Entities;

namespace Order.Infrastructure.Context
{
    public static class DatabaseSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            if (context.Products.Any())
                return;

            // Produtos
            var notebook = Product.Create("Notebook Dell Inspiron", 4599.90m).Value!;
            var mouse = Product.Create("Mouse Logitech MX Master", 549.90m).Value!;
            var teclado = Product.Create("Teclado Mecânico Redragon", 289.90m).Value!;
            var monitor = Product.Create("Monitor LG 27\" 4K", 2199.90m).Value!;
            var headset = Product.Create("Headset HyperX Cloud II", 399.90m).Value!;

            context.Products.AddRange(notebook, mouse, teclado, monitor, headset);
            context.SaveChanges();

            // Cupons
            var coupon10 = Coupon.Create("DESCONTO10", "10% de desconto", 0, 10, 100, DateTime.UtcNow.AddMonths(6)).Value!;
            var coupon50 = Coupon.Create("PROMO50", "R$50 de desconto", 50, 0, 50, DateTime.UtcNow.AddMonths(3)).Value!;
            var couponCombo = Coupon.Create("COMBO25", "25% até R$200", 0, 25, 200, DateTime.UtcNow.AddMonths(12)).Value!;

            context.Coupons.AddRange(coupon10, coupon50, couponCombo);
            context.SaveChanges();

            // Pedidos
            var items1 = new List<OrderItem>
            {
                new OrderItem(notebook, 1),
                new OrderItem(mouse, 2)
            };
            var order1 = Domain.Entities.Order.Create(1001, items1).Value!;

            var items2 = new List<OrderItem>
            {
                new OrderItem(monitor, 1),
                new OrderItem(teclado, 1),
                new OrderItem(headset, 1)
            };
            var order2 = Domain.Entities.Order.Create(1002, items2).Value!;
            order2.ApplyCoupon(coupon10);

            var items3 = new List<OrderItem>
            {
                new OrderItem(mouse, 3)
            };
            var order3 = Domain.Entities.Order.Create(1003, items3).Value!;
            order3.CloseOrder();

            context.Orders.AddRange(order1, order2, order3);
            context.SaveChanges();
        }
    }
}
