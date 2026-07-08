using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Repositories;
using EWallet.Worker.Projections.Interfaces;

namespace EWallet.Worker.Projections
{
    public class AccountBlockedProjection : IEventProjection
    {
        public string EventType => nameof(AccountBlocked);

        public async Task ProjectAsync(EventEnvelope envelope, IAccountReadRepository repository)
        {
            var account = await repository.GetByIdAsync(envelope.EntityId);
            if (account is null) return;

            account.Status = "Blocked";
            account.UpdatedAt = envelope.OccurredOn;

            await repository.UpsertAsync(account);
        }
    }
}
