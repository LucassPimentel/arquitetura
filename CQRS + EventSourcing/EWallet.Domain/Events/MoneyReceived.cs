using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class MoneyReceived : DomainEvent
    {
        public Guid DestinationAccountId { get; set; }
        public Guid SourceAccountId { get; set; }
        public decimal Amount { get; set; }

        public MoneyReceived(Guid destinationAccountId, Guid sourceAccountId, decimal amount)
        {
            DestinationAccountId = destinationAccountId;
            SourceAccountId = sourceAccountId;
            Amount = amount;
            EventType = nameof(MoneyReceived);
            EntityId = destinationAccountId;
        }
    }
}
