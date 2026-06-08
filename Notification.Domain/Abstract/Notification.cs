namespace Notification.Domain.Abstract
{
    public abstract class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Channel { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? SentAt { get; set; }
        public NotificationStatus Status { get; set; } = NotificationStatus.Pending;
        public string? ErrorMessage { get; set; }

        public void MarkAsSent()
        {
            Status = NotificationStatus.Sent;
            SentAt = DateTime.UtcNow;
            OnAfterSend();
        }

        public void MarkAsFailed(string errorMessage)
        {
            Status = NotificationStatus.Failed;
            ErrorMessage = errorMessage;
            SentAt = DateTime.UtcNow;
        }

        /// <summary>
        /// Valida a notificação específica do canal
        /// </summary>
        /// <exception cref="ArgumentException">Quando a validação falha</exception>
        public abstract void Validate();

        /// <summary>
        /// Retorna o tipo de notificação
        /// </summary>
        public abstract string GetNotificationType();

        /// <summary>
        /// Método para execução de lógica customizada antes do envio
        /// </summary>
        public virtual void OnBeforeSend() { }

        /// <summary>
        /// Método para execução após envio bem-sucedido
        /// </summary>
        public virtual void OnAfterSend() { }
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed,
        Retry
    }
}
