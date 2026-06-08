namespace Notification.Domain.Ports.Out
{
    public interface INotificationGateway
    {
        string Channel { get; }
        Task SendAsync(Abstract.Notification notification);
    }
}
