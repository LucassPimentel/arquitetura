using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class MoneyDeposited : DomainEvent
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal NewBalance { get; set; }

        public MoneyDeposited(Guid accountId, decimal amount, decimal newBalance)
        {
            AccountId = accountId;
            Amount = amount;
            NewBalance = newBalance;
            EventType = nameof(MoneyDeposited);
            EntityId = accountId;
        }
    }
}
