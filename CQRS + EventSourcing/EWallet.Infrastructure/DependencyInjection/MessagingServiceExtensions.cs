using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Messaging.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace EWallet.Infrastructure.DependencyInjection
{
    public static class MessagingServiceExtensions
    {
        /// <summary>
        /// Registra os serviços de mensageria (RabbitMQ): conexão e publisher.
        /// Os consumers rodam em Worker Services separados.
        /// </summary>
        public static IServiceCollection AddRabbitMqPublisher(
            this IServiceCollection services,
            Action<RabbitMqSettings>? configureSettings = null)
        {
            var settings = new RabbitMqSettings();
            configureSettings?.Invoke(settings);

            services.AddSingleton(settings);
            services.AddSingleton<RabbitMqConnection>();
            services.AddScoped<IEventPublisher, RabbitMqEventPublisher>();

            return services;
        }
    }
}
