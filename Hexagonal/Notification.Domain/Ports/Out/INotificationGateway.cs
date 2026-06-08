namespace Notification.Domain.Ports.Out
{
    public interface INotificationGateway : IChannelComponent
    {
        Task SendAsync(Abstract.Notification notification);
    }
}
