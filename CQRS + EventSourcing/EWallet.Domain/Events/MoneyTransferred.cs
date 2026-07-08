using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class MoneyTransferred : DomainEvent
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }

        public MoneyTransferred(Guid sourceAccountId, Guid destinationAccountId, decimal amount)
        {
            SourceAccountId = sourceAccountId;
            DestinationAccountId = destinationAccountId;
            Amount = amount;
            EventType = nameof(MoneyTransferred);
            EntityId = sourceAccountId;
        }
    }
}
