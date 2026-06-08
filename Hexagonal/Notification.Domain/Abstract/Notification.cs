using System.ComponentModel.DataAnnotations;

namespace Notification.Domain.Abstract
{
    public abstract class Notification
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Channel { get; set; } = string.Empty;
        public string Recipient { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
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
        /// Template method que define a sequência fixa de validação de uma notificação.
        /// As subclasses fornecem os passos é um passo
        /// opcional (hook) que cada canal sobrescreve apenas se precisar.
        /// </summary>
        /// <exception cref="ArgumentException">Quando a validação falha</exception>
        public void Validate()
        {
            ValidateRecipient();
            ValidateMessage();
            ValidateSubject();
        }

        /// <summary>
        /// Valida o destinatário de acordo com as regras do canal.
        /// </summary>
        /// <exception cref="ArgumentException">Quando o destinatário é inválido</exception>
        protected abstract void ValidateRecipient();

        /// <summary>
        /// Valida o conteúdo da mensagem de acordo com as regras do canal.
        /// </summary>
        /// <exception cref="ArgumentException">Quando a mensagem é inválida</exception>
        protected abstract void ValidateMessage();

        /// <summary>
        /// Passo opcional de validação do assunto. A implementação padrão não faz nada;
        /// canais que exigem assunto (ex.: Email) sobrescrevem este método.
        /// </summary>
        /// <exception cref="ArgumentException">Quando o assunto é inválido</exception>
        protected virtual void ValidateSubject() { }

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
