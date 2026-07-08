using System.Text.Json;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Repositories;
using EWallet.Worker.Projections.Interfaces;

namespace EWallet.Worker.Projections
{
    public class MoneyRefundReceivedProjection : IEventProjection
    {
        public string EventType => nameof(MoneyRefundReceived);

        public async Task ProjectAsync(EventEnvelope envelope, IAccountReadRepository repository)
        {
            var @event = JsonSerializer.Deserialize<MoneyRefundReceived>(envelope.EventData);
            if (@event is null) return;

            var account = await repository.GetByIdAsync(envelope.EntityId);
            if (account is null) return;

            account.Balance += @event.Amount;
            account.UpdatedAt = envelope.OccurredOn;

            await repository.UpsertAsync(account);
        }
    }
}
