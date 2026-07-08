using System.Text.Json;
using EWallet.Application.Events;
using EWallet.Domain.Entities;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Repositories;
using EWallet.Worker.Projections.Interfaces;

namespace EWallet.Worker.Projections
{
    public class AccountCreatedProjection : IEventProjection
    {
        public string EventType => nameof(AccountCreated);

        public async Task ProjectAsync(EventEnvelope envelope, IAccountReadRepository repository)
        {
            var @event = JsonSerializer.Deserialize<AccountCreated>(envelope.EventData);
            if (@event is null) return;

            var account = new AccountReadModel
            {
                Id = envelope.EntityId,
                UserName = @event.AccountName,
                Balance = 0,
                Status = "Active",
                CreatedAt = envelope.OccurredOn,
                UpdatedAt = envelope.OccurredOn
            };

            await repository.UpsertAsync(account);
        }
    }
}
