using Notification.Application.DTOs;
using Notification.Application.Factories.Interfaces;
using Notification.Domain.Entities;
using Notification.Domain.ValueObjects;

namespace Notification.Application.Factories
{
    /// <summary>
    /// Fábrica concreta que produz a família de produtos do canal <b>Email</b>.
    /// </summary>
    public class EmailNotificationFactory : IChannelNotificationFactory
    {
        public string Channel => "Email";

        public Domain.Abstract.Notification CreateNotification(CreateNotificationRequest request)
            => new EmailNotification(request.Message, request.Subject, request.Recipient);

        public ChannelSpecification CreateSpecification() => new()
        {
            DisplayName = "Email",
            RecipientPlaceholder = "exemplo@email.com",
            RequiresSubject = true
        };
    }
}
