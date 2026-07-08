using EWallet.Domain.Entities;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Queries.Handlers
{
    public class GetEventsByAccountHandler : IRequestHandler<GetEventsByAccountQuery, IEnumerable<EventRecord>>
    {
        private readonly IEventStoreRepository _eventStore;

        public GetEventsByAccountHandler(IEventStoreRepository eventStore)
        {
            _eventStore = eventStore;
        }

        public Task<IEnumerable<EventRecord>> Handle(GetEventsByAccountQuery request, CancellationToken cancellationToken)
            => _eventStore.GetAllEventsByEntityAsync(request.AccountId);
    }
}
