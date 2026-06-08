using Notification.Application.DTOs;
using Notification.Application.Factories.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories
{
    /// <summary>
    /// Fábrica concreta que produz a família de produtos do canal <b>WhatsApp</b>.
    /// </summary>
    public class WhatsAppNotificationFactory : IChannelNotificationFactory
    {
        public string Channel => "WhatsApp";

        public Domain.Abstract.Notification CreateNotification(CreateNotificationRequest request)
            => new WhatsAppNotification(request.Message, request.Subject, request.Recipient);

        public ChannelSpecification CreateSpecification() => new()
        {
            DisplayName = "WhatsApp",
            RecipientPlaceholder = "+55 11 99999-9999",
            RequiresSubject = false
        };
    }
}
