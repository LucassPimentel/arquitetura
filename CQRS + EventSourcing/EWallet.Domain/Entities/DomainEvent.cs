namespace EWallet.Domain.Entities
{
    public abstract class DomainEvent
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid EntityId { get; set; }
        public string EventType { get; set; }
        public DateTime EventDate { get; set; } = DateTime.UtcNow;
    }
}
