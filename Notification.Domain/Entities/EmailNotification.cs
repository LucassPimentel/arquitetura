using System.Net.Mail;

namespace Notification.Domain.Entities
{
    public class EmailNotification : Abstract.Notification
    {
        public EmailNotification(string message, string subject, string recipient)
        {
            Channel = "Email";
            Message = message;
            Subject = subject;
            Recipient = recipient;
        }

        public override void Validate()
        {
            ValidateRecipient();
            ValidateMessage();
            ValidateSubject();
        }

        private void ValidateRecipient()
        {
            if (string.IsNullOrWhiteSpace(Recipient))
                throw new ArgumentException("Email destinatário é obrigatório", nameof(Recipient));

            if (!IsValidEmail(Recipient))
                throw new ArgumentException($"Email inválido: {Recipient}", nameof(Recipient));
        }

        private void ValidateMessage()
        {
            if (string.IsNullOrWhiteSpace(Message))
                throw new ArgumentException("Mensagem é obrigatória", nameof(Message));

            if (Message.Length > 10000)
                throw new ArgumentException("Mensagem não pode ter mais de 10.000 caracteres", nameof(Message));
        }

        private void ValidateSubject()
        {
            if (string.IsNullOrWhiteSpace(Subject))
                throw new ArgumentException("Assunto é obrigatório para email", nameof(Subject));

            if (Subject.Length > 200)
                throw new ArgumentException("Assunto não pode ter mais de 200 caracteres", nameof(Subject));
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var mailAddress = new MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public override string GetNotificationType() => "Email";

        public override void OnBeforeSend()
        {
            Message = Message.Trim();
            Subject = Subject.Trim();
        }
    }
}