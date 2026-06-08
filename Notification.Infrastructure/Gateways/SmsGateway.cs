using Microsoft.Extensions.Logging;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Gateways
{
    public class SmsGateway : Domain.Ports.Out.INotificationGateway
    {
        public string Channel => "SMS";
        private readonly ILogger<SmsGateway> _logger;

        public SmsGateway(ILogger<SmsGateway> logger)
        {
            _logger = logger;
        }


        public async Task SendAsync(Domain.Abstract.Notification notification)
        {
            try
            {
                // Cast seguro para SmsNotification
                if (notification is not SmsNotification smsNotification)
                    throw new InvalidOperationException($"SmsGateway esperava SmsNotification, recebeu {notification.GetType().Name}");

                // Executar validações específicas
                smsNotification.Validate();
                smsNotification.OnBeforeSend();

                // Simulação de envio de SMS
                _logger.LogInformation($"SMS enviado para {smsNotification.Recipient}: {smsNotification.Message}");
                _logger.LogInformation($"Partes necessárias: {smsNotification.GetPartCount()}");

                smsNotification.MarkAsSent();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                _logger.LogError($"Erro ao enviar SMS: {ex.Message}", ex);
                throw;
            }
        }
    }
}

