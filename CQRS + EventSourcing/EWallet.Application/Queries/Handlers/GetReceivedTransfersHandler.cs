using EWallet.Domain.Entities;
using EWallet.Domain.Events;
using EWallet.Domain.Helpers;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Queries.Handlers
{
    public class GetReceivedTransfersHandler : IRequestHandler<GetReceivedTransfersQuery, IEnumerable<MoneyReceived>>
    {
        private readonly IEventStoreRepository _eventStore;

        public GetReceivedTransfersHandler(IEventStoreRepository eventStore)
        {
            _eventStore = eventStore;
        }

        public async Task<IEnumerable<MoneyReceived>> Handle(GetReceivedTransfersQuery request, CancellationToken cancellationToken)
        {
            var records = await _eventStore.GetAllEventsByEntityAsync(request.AccountId);

            return records
                .Select(r => EventDeserializer.Deserialize(r))
                .OfType<MoneyReceived>()
                .ToList();
        }
    }
}
