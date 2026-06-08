using Microsoft.Extensions.Logging;
using Notification.Domain.Entities;

namespace Notification.Infrastructure.Gateways
{
    public class EmailGateway : Domain.Ports.Out.INotificationGateway
    {
        public string Channel => "Email";
        private readonly ILogger<EmailGateway> _logger;

        public EmailGateway(ILogger<EmailGateway> logger)
        {
            _logger = logger;
        }


        public async Task SendAsync(Domain.Abstract.Notification notification)
        {
            try
            {
                // Cast seguro para EmailNotification
                if (notification is not EmailNotification emailNotification)
                    throw new InvalidOperationException($"EmailGateway esperava EmailNotification, recebeu {notification.GetType().Name}");

                // Executar validações específicas
                emailNotification.Validate();
                emailNotification.OnBeforeSend();

                // Simulação de envio de Email
                _logger.LogInformation($"Email enviado para {emailNotification.Recipient}");
                _logger.LogInformation($"Assunto: {emailNotification.Subject}");
                _logger.LogInformation($"Mensagem: {emailNotification.Message}");

                emailNotification.MarkAsSent();

                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                notification.MarkAsFailed(ex.Message);
                _logger.LogError($"Erro ao enviar email: {ex.Message}", ex);
                throw;
            }
        }
    }
}
