using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class MoneyRefunded : DomainEvent
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public Guid OriginalTransferEventId { get; set; }

        public MoneyRefunded(Guid sourceAccountId, Guid destinationAccountId, decimal amount, Guid originalTransferEventId)
        {
            SourceAccountId = sourceAccountId;
            DestinationAccountId = destinationAccountId;
            Amount = amount;
            OriginalTransferEventId = originalTransferEventId;
            EventType = nameof(MoneyRefunded);
            EntityId = sourceAccountId;
        }
    }
}
