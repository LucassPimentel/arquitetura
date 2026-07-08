using EWallet.Domain.Entities;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Commands.Handlers
{
    public class RefundHandler : IRequestHandler<RefundCommand, bool>
    {
        private readonly IEventStoreRepository _eventStore;
        private readonly IEventPublisher _eventPublisher;

        public RefundHandler(
            IEventStoreRepository eventStore,
            IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public async Task<bool> Handle(RefundCommand request, CancellationToken cancellationToken)
        {
            var sourceRecords = await _eventStore.GetEventsAsync(request.SourceAccountId, fromVersion: 0);
            if (!sourceRecords.Any())
                throw new InvalidOperationException($"Conta de origem {request.SourceAccountId} não encontrada.");

            var destinationRecords = await _eventStore.GetEventsAsync(request.DestinationAccountId, fromVersion: 0);
            if (!destinationRecords.Any())
                throw new InvalidOperationException($"Conta de destino {request.DestinationAccountId} não encontrada.");

            var source = Account.Rehydrate(sourceRecords);
            var destination = Account.Rehydrate(destinationRecords);

            source.Refund(destination, request.Amount, request.OriginalTransferEventId);

            var refundedEvent = new MoneyRefunded(request.SourceAccountId, request.DestinationAccountId, request.Amount, request.OriginalTransferEventId);
            var refundReceivedEvent = new MoneyRefundReceived(request.DestinationAccountId, request.SourceAccountId, request.Amount);

            await _eventStore.SaveEventAsync(request.SourceAccountId, refundedEvent, source.Version);
            await _eventStore.SaveEventAsync(request.DestinationAccountId, refundReceivedEvent, destination.Version);

            await _eventPublisher.PublishAsync(request.SourceAccountId, refundedEvent);
            await _eventPublisher.PublishAsync(request.DestinationAccountId, refundReceivedEvent);

            return true;
        }
    }
}
