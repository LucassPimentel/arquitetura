using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Interfaces;
using Order.Application.Mappings;
using Order.Application.Services;
using Order.Domain.Interfaces.Repositories;
using Order.Infrastructure.Context;
using Order.Infrastructure.Repositories;

namespace Order.CrossCutting.IoC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddDbContext<AppDbContext>(options =>
                options.UseInMemoryDatabase("OrderDb"));

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<ICouponRepository, CouponRepository>();
            services.AddScoped<ICouponService, CouponService>();
            
            // Registrar AutoMapper profiles
            services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<OrderMappingProfile>();
                cfg.AddProfile<ProductMappingProfile>();
                cfg.AddProfile<CouponMappingProfile>();
            });

            return services;
        }
    }
}
