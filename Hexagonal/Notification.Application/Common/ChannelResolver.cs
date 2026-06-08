using Microsoft.Extensions.DependencyInjection;
using Notification.Domain.Ports;

namespace Notification.Application.Common
{
    /// <summary>
    /// Implementação de <see cref="IChannelResolver"/> que descobre os componentes
    /// registrados no contêiner via <see cref="IServiceProvider"/> e seleciona o que
    /// atende o canal informado.
    /// </summary>
    public class ChannelResolver : IChannelResolver
    {
        private readonly IServiceProvider _provider;

        public ChannelResolver(IServiceProvider provider)
            => _provider = provider;

        public TComponent Resolve<TComponent>(string channel) where TComponent : IChannelComponent
            => TryResolve<TComponent>(channel)
               ?? throw new ArgumentException($"Canal inválido: {channel}");

        public TComponent? TryResolve<TComponent>(string channel) where TComponent : IChannelComponent
            => _provider.GetServices<TComponent>()
                        .FirstOrDefault(c => c.Channel == channel);
    }
}
