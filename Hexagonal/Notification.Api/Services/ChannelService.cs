using Notification.Blazor.Models;
using Notification.Domain.Ports.Out;

namespace Notification.Blazor.Services;

public class ChannelService
{
    private readonly List<ChannelModel> _channels = new();

    public ChannelService(IEnumerable<INotificationGateway> gateways)
    {
        DiscoverChannels(gateways);
    }

    private void DiscoverChannels(IEnumerable<INotificationGateway> gateways)
    {
        foreach (var gateway in gateways)
        {
            var channelName = gateway.Channel; // usa a propriedade da interface
            _channels.Add(new ChannelModel
            {
                Name = channelName,
                Adapter = gateway.GetType().Name,
                UseCase = "SendNotificationUseCase",
                IsAvailable = true
            });
        }

        _channels.Sort((a, b) => a.Name.CompareTo(b.Name));
    }
    public List<ChannelModel> GetChannels() => _channels;
}