using EWallet.Domain.Entities;

namespace EWallet.Application.Events
{
    public class AccountCreated : DomainEvent
    {
        public string AccountName { get; private set; } = string.Empty;

        public AccountCreated(Guid entityId, string accountName)
        {
            AccountName = accountName;
            EventType = nameof(AccountCreated);
            EntityId = entityId;
        }
    }
}
