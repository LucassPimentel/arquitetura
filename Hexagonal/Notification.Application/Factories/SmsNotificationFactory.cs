using Notification.Application.DTOs;
using Notification.Application.Factories.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories
{
    /// <summary>
    /// Fábrica concreta que produz a família de produtos do canal <b>SMS</b>.
    /// </summary>
    public class SmsNotificationFactory : IChannelNotificationFactory
    {
        public string Channel => "SMS";

        public Domain.Abstract.Notification CreateNotification(CreateNotificationRequest request)
            => new SmsNotification(request.Message, request.Subject, request.Recipient);

        public ChannelSpecification CreateSpecification() => new()
        {
            DisplayName = "SMS",
            RecipientPlaceholder = "+55 11 99999-9999",
            RequiresSubject = false
        };
    }
}
