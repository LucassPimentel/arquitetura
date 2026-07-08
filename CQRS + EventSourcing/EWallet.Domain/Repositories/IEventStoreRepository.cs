using EWallet.Domain.Entities;

namespace EWallet.Infrastructure.Persistance.Event_Store
{
    public interface IEventStoreRepository
    {
        Task<IEnumerable<EventRecord>> GetEventsAsync(Guid entityId, int fromVersion);
        Task SaveEventAsync(Guid entityId, DomainEvent @event, int expectedVersion);
        Task<IEnumerable<EventRecord>> GetAllEventsByEntityAsync(Guid entityId);
    }
}
