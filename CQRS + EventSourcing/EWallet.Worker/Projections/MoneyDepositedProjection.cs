using System.Text.Json;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Repositories;
using EWallet.Worker.Projections.Interfaces;

namespace EWallet.Worker.Projections
{
    public class MoneyDepositedProjection : IEventProjection
    {
        public string EventType => nameof(MoneyDeposited);

        public async Task ProjectAsync(EventEnvelope envelope, IAccountReadRepository repository)
        {
            var @event = JsonSerializer.Deserialize<MoneyDeposited>(envelope.EventData);
            if (@event is null) return;

            var account = await repository.GetByIdAsync(envelope.EntityId);
            if (account is null) return;

            account.Balance = @event.NewBalance;
            account.UpdatedAt = envelope.OccurredOn;

            await repository.UpsertAsync(account);
        }
    }
}
