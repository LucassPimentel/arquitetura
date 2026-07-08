namespace EWallet.Domain.Entities
{
    public class EventRecord
    {
        public Guid Id { get; set; }
        public Guid EntityId { get; set; }
        public int Version { get; set; }
        public string EventType { get; set; }
        public string EventData { get; set; }
        public DateTime EventDate { get; set; } = DateTime.UtcNow;
    }
}
