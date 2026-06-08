using System.Text.RegularExpressions;

namespace Notification.Domain.Entities
{
    public class WhatsAppNotification : Abstract.Notification
    {
        /// <summary>
        /// Comprimento máximo de mensagem WhatsApp
        /// </summary>
        public const int MaxWhatsAppLength = 4096;

        public WhatsAppNotification(string message, string subject, string recipient)
        {
            Channel = "WhatsApp";
            Message = message;
            Subject = subject;
            Recipient = recipient;
        }

        protected override void ValidateRecipient()
        {
            if (string.IsNullOrWhiteSpace(Recipient))
                throw new ArgumentException("Número WhatsApp é obrigatório", nameof(Recipient));

            if (!IsValidPhoneNumber(Recipient))
                throw new ArgumentException($"Número de telefone inválido para WhatsApp: {Recipient}", nameof(Recipient));
        }

        protected override void ValidateMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
                throw new ArgumentException("Mensagem é obrigatória", nameof(Message));

            if (Message.Length > MaxWhatsAppLength)
                throw new ArgumentException($"Mensagem WhatsApp não pode ter mais de {MaxWhatsAppLength} caracteres", 
                    nameof(Message));
        }

        private bool IsValidPhoneNumber(string phoneNumber)
        {
            var cleaned = Regex.Replace(phoneNumber, @"\D", "");
            
            // E.164: entre 10 e 15 dígitos
            return cleaned.Length >= 10 && cleaned.Length <= 15;
        }

        public override string GetNotificationType() => "WhatsApp";

        public override void OnBeforeSend()
        {
            // Preparar formatação para WhatsApp
            Message = Message.Trim();
            
            // Subject não é usado no WhatsApp
            Subject = string.Empty;
        }

        public override void OnAfterSend()
        {
            // Log de entrega de mensagem WhatsApp
        }

        /// <summary>
        /// Valida se o número está no formato E.164 (com +)
        /// </summary>
        public bool IsE164Format() => Recipient.StartsWith("+");

        /// <summary>
        /// Converte para formato E.164 se necessário
        /// </summary>
        public string GetE164PhoneNumber()
        {
            var cleaned = Regex.Replace(Recipient, @"\D", "");
            
            if (!cleaned.StartsWith("55")) // Brasil
            {
                cleaned = "55" + cleaned;
            }
            
            return "+" + cleaned;
        }
    }
}
