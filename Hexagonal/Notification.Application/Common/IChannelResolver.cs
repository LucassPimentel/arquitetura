using Notification.Domain.Ports;

namespace Notification.Application.Common
{
    /// <summary>
    /// Resolve um componente especializado em um canal a partir das implementações
    /// Centraliza a lógica de "resolver por canal" usada por casos de uso e fábricas.
    /// </summary>
    public interface IChannelResolver
    {
        /// <summary>
        /// Resolve o componente do canal informado.
        /// </summary>
        /// <exception cref="ArgumentException">Quando nenhum componente atende o canal.</exception>
        TComponent Resolve<TComponent>(string channel) where TComponent : IChannelComponent;

        /// <summary>
        /// Tenta resolver o componente do canal informado, retornando <c>null</c> quando não há um correspondente.
        /// </summary>
        TComponent? TryResolve<TComponent>(string channel) where TComponent : IChannelComponent;
    }
}
