using EWallet.Infrastructure.Mongo;
using EWallet.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;

namespace EWallet.Infrastructure.DependencyInjection
{
    public static class MongoServiceExtensions
    {
        public static IServiceCollection AddMongoDb(
            this IServiceCollection services,
            Action<MongoSettings>? configure = null)
        {
            var settings = new MongoSettings();
            configure?.Invoke(settings);

            services.AddSingleton(settings);
            services.AddSingleton<IMongoClient>(sp => new MongoClient(settings.ConnectionString));
            services.AddSingleton(sp => sp.GetRequiredService<IMongoClient>().GetDatabase(settings.DatabaseName));

            // Register repositories
            services.AddScoped<Repositories.IAccountReadRepository, AccountReadRepository>();

            return services;
        }
    }
}
