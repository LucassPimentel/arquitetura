using System.Text.RegularExpressions;

namespace Notification.Domain.Entities
{
    public class SmsNotification : Abstract.Notification
    {
        /// <summary>
        /// Comprimento máximo de um SMS (160 caracteres para ASCII, 70 para Unicode)
        /// </summary>
        public const int MaxSmsLength = 160;

        public SmsNotification(string message, string subject, string recipient)
        {
            Channel = "SMS";
            Message = message;
            Subject = subject;
            Recipient = recipient;
        }

        public override void Validate()
        {
            ValidateRecipient();
            ValidateMessage();
        }

        private void ValidateRecipient()
        {
            if (string.IsNullOrWhiteSpace(Recipient))
                throw new ArgumentException("Número de telefone é obrigatório", nameof(Recipient));

            if (!IsValidPhoneNumber(Recipient))
                throw new ArgumentException($"Número de telefone inválido: {Recipient}", nameof(Recipient));
        }

        private void ValidateMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
                throw new ArgumentException("Mensagem é obrigatória", nameof(Message));

            if (Message.Length > MaxSmsLength)
                throw new ArgumentException($"Mensagem SMS não pode ter mais de {MaxSmsLength} caracteres. " +
                    $"Tamanho atual: {Message.Length}", nameof(Message));
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var cleaned = Regex.Replace(phoneNumber, @"\D", "");
            
            return cleaned.Length >= 10 && cleaned.Length <= 15;
        }

        public override string GetNotificationType() => "SMS";

        public override void OnBeforeSend()
        {
            // Truncar se necessário (considerar caracteres especiais)
            if (Message.Length > MaxSmsLength)
            {
                Message = Message.Substring(0, MaxSmsLength).Trim();
            }
        }

        public override void OnAfterSend()
        {
            // Log de entrega de SMS
        }

        /// <summary>
        /// Retorna o número de SMS necessários para enviar a mensagem completa
        /// </summary>
        public int GetPartCount() => (int)Math.Ceiling((double)Message.Length / MaxSmsLength);
    }
}
