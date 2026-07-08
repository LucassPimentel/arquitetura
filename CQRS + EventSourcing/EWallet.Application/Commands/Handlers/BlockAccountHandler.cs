using EWallet.Domain.Entities;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Commands.Handlers
{
    public class BlockAccountHandler : IRequestHandler<BlockAccountCommand, bool>
    {
        private readonly IEventStoreRepository _eventStore;
        private readonly IEventPublisher _eventPublisher;

        public BlockAccountHandler(
            IEventStoreRepository eventStore,
            IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(BlockAccountCommand request, CancellationToken cancellationToken)
        {
            var records = await _eventStore.GetEventsAsync(request.AccountId, fromVersion: 0);
            if (!records.Any())
                throw new InvalidOperationException($"Conta {request.AccountId} não encontrada.");

            var account = Account.Rehydrate(records);

            account.BlockAccount();

            var blockedEvent = new AccountBlocked(request.AccountId);

            await _eventStore.SaveEventAsync(request.AccountId, blockedEvent, account.Version);
            await _eventPublisher.PublishAsync(request.AccountId, blockedEvent);

            return true;
        }
    }
}
