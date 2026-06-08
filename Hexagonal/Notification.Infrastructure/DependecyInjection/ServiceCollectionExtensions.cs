using Microsoft.Extensions.DependencyInjection;
using Notification.Infrastructure.Gateways;
using Notification.Domain.Ports.Out;

namespace Notification.Infrastructure.DependecyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddScoped<INotificationGateway, EmailGateway>();
            services.AddScoped<INotificationGateway, SmsGateway>();
            services.AddScoped<INotificationGateway, WhatsAppGateway>();

            return services;
        }
    }
}
