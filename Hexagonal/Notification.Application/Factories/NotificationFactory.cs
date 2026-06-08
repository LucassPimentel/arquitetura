using Notification.Application.Common;
using Notification.Application.DTOs;
using Notification.Application.Factories.Interfaces;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories
{
    /// <summary>
    /// Essa fábrica é responsável por criar notificações de diferentes canais (Email, SMS, WhatsApp, etc.) com base no tipo de canal especificado no request. Ela utiliza o padrão Factory Method para delegar a criação da notificação para as fábricas específicas de cada canal, garantindo assim a flexibilidade e extensibilidade do sistema. (com ela substituímos a necessidade de um switch case ou if-else para cada tipo de canal, facilitando a manutenção e adição de novos canais no futuro).
    /// </summary>
    public class NotificationFactory : INotificationFactory
    {
        private readonly IChannelResolver _resolver;

        public NotificationFactory(IChannelResolver resolver)
            => _resolver = resolver;

        public Domain.Abstract.Notification CreateNotification(CreateNotificationRequest request)
            => _resolver.Resolve<IChannelNotificationFactory>(request.Channel).CreateNotification(request);

        public ChannelSpecification GetSpecification(string channel)
            => _resolver.Resolve<IChannelNotificationFactory>(channel).CreateSpecification();
    }
}
