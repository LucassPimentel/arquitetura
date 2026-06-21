using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Social.Infrastructure.Persistence
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection")
                                   ?? Environment.GetEnvironmentVariable("DefaultConnection")
                                   ?? "Server=db,1433;Database=SocialDb;User Id=sa;Password=Your_password123;TrustServerCertificate=True;";

            services.AddDbContext<SocialDbContext>(options => options.UseSqlServer(connectionString));

            return services;
        }
    }
}
