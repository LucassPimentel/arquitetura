namespace Notification.Domain.Ports
{
    /// <summary>
    /// Contrato comum para qualquer componente especializado em um canal
    /// (gateways de envio, fábricas de notificação, etc.).
    /// Permite resolver a implementação correta a partir do nome do canal.
    /// </summary>
    public interface IChannelComponent
    {
        /// <summary>Nome do canal atendido (ex.: "Email", "SMS", "WhatsApp").</summary>
        string Channel { get; }
    }
}
