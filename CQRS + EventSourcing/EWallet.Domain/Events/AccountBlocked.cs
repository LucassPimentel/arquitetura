using EWallet.Domain.Entities;

namespace EWallet.Domain.Events
{
    public class AccountBlocked : DomainEvent
    {
        public Guid AccountId { get; set; }

        public AccountBlocked(Guid accountId)
        {
            AccountId = accountId;
            EventType = nameof(AccountBlocked);
            EntityId = accountId;
        }
    }
}
