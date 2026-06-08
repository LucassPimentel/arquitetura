using Microsoft.Extensions.Logging;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Gateways
{
    public class WhatsAppGateway : Domain.Ports.Out.INotificationGateway
    {
        public string Channel => "WhatsApp";
        private readonly ILogger<WhatsAppGateway> _logger;

        public WhatsAppGateway(ILogger<WhatsAppGateway> logger)
        {
            _logger = logger;
        }

        public async Task SendAsync(Domain.Abstract.Notification notification)
        {
            try
            {
                // Cast seguro para WhatsAppNotification
                if (notification is not WhatsAppNotification whatsAppNotification)
                    throw new InvalidOperationException($"WhatsAppGateway esperava WhatsAppNotification, recebeu {notification.GetType().Name}");

                // Executar validações específicas
                whatsAppNotification.Validate();
                whatsAppNotification.OnBeforeSend();

                // Simulação de envio de WhatsApp
                _logger.LogInformation($"Mensagem WhatsApp enviada para {whatsAppNotification.GetE164PhoneNumber()}");
                _logger.LogInformation($"Mensagem: {whatsAppNotification.Message}");

                whatsAppNotification.MarkAsSent();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                _logger.LogError($"Erro ao enviar mensagem WhatsApp: {ex.Message}", ex);
                throw;
            }
        }
    }
}

