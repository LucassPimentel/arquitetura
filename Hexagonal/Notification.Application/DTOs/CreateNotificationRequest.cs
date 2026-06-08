namespace Notification.Application.DTOs
{
    public record CreateNotificationRequest(
        string Channel,
        string Recipient,
        string Subject,
        string Message
    );
}
