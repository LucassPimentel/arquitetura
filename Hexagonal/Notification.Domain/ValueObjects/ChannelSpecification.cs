namespace Notification.Domain.ValueObjects
{
    public record ChannelSpecification
    {
        /// <summary>Nome de exibição do canal.</summary>
        public required string DisplayName { get; init; }

        /// <summary>Exemplo de formato esperado para o destinatário (usado como placeholder).</summary>
        public required string RecipientPlaceholder { get; init; }

        /// <summary>Indica se o canal utiliza o campo Assunto.</summary>
        public required bool RequiresSubject { get; init; }
    }
}
