namespace Notification.Domain.Ports.In
{
    public interface ISendNotificationUseCase
    {
        Task Execute(Abstract.Notification notification);
    }
}
