using EWallet.Domain.Entities;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Commands.Handlers
{
    public class TransferHandler : IRequestHandler<TransferCommand, bool>
    {
        private readonly IEventStoreRepository _eventStore;
        private readonly IEventPublisher _eventPublisher;

        public TransferHandler(
            IEventStoreRepository eventStore,
            IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(TransferCommand request, CancellationToken cancellationToken)
        {
            var sourceRecords = await _eventStore.GetEventsAsync(request.SourceAccountId, fromVersion: 0);
            if (!sourceRecords.Any())
                throw new InvalidOperationException($"Conta de origem {request.SourceAccountId} não encontrada.");

            var destinationRecords = await _eventStore.GetEventsAsync(request.DestinationAccountId, fromVersion: 0);
            if (!destinationRecords.Any())
                throw new InvalidOperationException($"Conta de destino {request.DestinationAccountId} não encontrada.");

            var source = Account.Rehydrate(sourceRecords);
            var destination = Account.Rehydrate(destinationRecords);

            source.Transfer(destination, request.Amount);

            var transferredEvent = new MoneyTransferred(request.SourceAccountId, request.DestinationAccountId, request.Amount);
            var receivedEvent = new MoneyReceived(request.DestinationAccountId, request.SourceAccountId, request.Amount);

            await _eventStore.SaveEventAsync(request.SourceAccountId, transferredEvent, source.Version);
            await _eventStore.SaveEventAsync(request.DestinationAccountId, receivedEvent, destination.Version);

            await _eventPublisher.PublishAsync(request.SourceAccountId, transferredEvent);
            await _eventPublisher.PublishAsync(request.DestinationAccountId, receivedEvent);

            return true;
        }
    }
}
