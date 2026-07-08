namespace EWallet.Infrastructure.Messaging
{
    /// <summary>
    /// Envelope que encapsula o evento para transporte via mensageria.
    /// Contém metadados necessários para reidratação e persistência.
    /// </summary>
    public class EventEnvelope
    {
        public Guid EntityId { get; set; }
        public Guid EventId { get; set; }
        public string EventType { get; set; } = string.Empty;
        public string EventData { get; set; } = string.Empty;
        public DateTime OccurredOn { get; set; }
    }
}
