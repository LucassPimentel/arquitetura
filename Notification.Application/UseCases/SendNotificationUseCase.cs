using Notification.Domain.Ports.Out;

namespace Notification.Application.UseCases
{
    public class SendNotificationUseCase : Domain.Ports.In.ISendNotificationUseCase
    {

        private readonly IEnumerable<INotificationGateway> _gateway;

        public SendNotificationUseCase(IEnumerable<INotificationGateway> gateway)
        {
            _gateway = gateway;
        }

        public async Task Execute(Domain.Abstract.Notification notification)
        {
            var gateway = _gateway.FirstOrDefault(g => g.Channel == notification.Channel);

            if (gateway != null)
            {
                await gateway.SendAsync(notification);
            }
        }
    }
}
