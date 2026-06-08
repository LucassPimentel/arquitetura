using Notification.Application.DTOs;
using Notification.Domain.Ports;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories.Interfaces
{
    public interface IChannelNotificationFactory : IChannelComponent
    {
        /// <summary>Cria a entidade de notificação específica do canal.</summary>
        Domain.Abstract.Notification CreateNotification(CreateNotificationRequest request);

        /// <summary>Cria a especificação (metadados) do canal.</summary>
        ChannelSpecification CreateSpecification();
    }
}
