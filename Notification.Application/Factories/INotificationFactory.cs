using Notification.Application.DTOs;

namespace Notification.Domain.Interfaces.Factories
{
    public interface INotificationFactory
    {
        Abstract.Notification CreateNotification(CreateNotificationRequest createNotificationRequest);
    }
}
