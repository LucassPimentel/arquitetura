using Notification.Application.DTOs;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories.Interfaces
{
    public interface INotificationFactory
    {
        /// <summary>Cria a entidade de notificação do canal informado no request.</summary>
        Domain.Abstract.Notification CreateNotification(CreateNotificationRequest createNotificationRequest);

        /// <summary>Obtém a especificação (metadados) do canal informado.</summary>
        ChannelSpecification GetSpecification(string channel);
    }
}
