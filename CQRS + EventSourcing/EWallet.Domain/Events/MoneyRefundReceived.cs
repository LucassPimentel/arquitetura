using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class MoneyRefundReceived : DomainEvent
    {
        public Guid DestinationAccountId { get; set; }
        public Guid SourceAccountId { get; set; }
        public decimal Amount { get; set; }

        public MoneyRefundReceived(Guid destinationAccountId, Guid sourceAccountId, decimal amount)
        {
            DestinationAccountId = destinationAccountId;
            SourceAccountId = sourceAccountId;
            Amount = amount;
            EventType = nameof(MoneyRefundReceived);
            EntityId = destinationAccountId;
        }
    }
}
