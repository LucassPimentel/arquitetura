using Notification.Application.Common;
using Notification.Domain.Ports.Out;

namespace Notification.Application.UseCases
{
    public class SendNotificationUseCase : Domain.Ports.In.ISendNotificationUseCase
    {

        private readonly IChannelResolver _resolver;

        public SendNotificationUseCase(IChannelResolver resolver)
        {
            _resolver = resolver;
        }

        public async Task Execute(Domain.Abstract.Notification notification)
        {
            var gateway = _resolver.TryResolve<INotificationGateway>(notification.Channel);

            if (gateway != null)
            {
                await gateway.SendAsync(notification);
            }
        }
    }
}
