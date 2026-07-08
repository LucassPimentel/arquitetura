using EWallet.Domain.Entities;
using EWallet.Domain.Exceptions;
using EWallet.Infrastructure.Context;
using EWallet.Infrastructure.Persistance.Event_Store;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace EWallet.Infrastructure.Repositories
{
    public class EventStoreRepository : IEventStoreRepository
    {
        private readonly EWalletDbContext _dbContext;

        public EventStoreRepository(EWalletDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task SaveEventAsync(Guid entityId, DomainEvent @event, int expectedVersion)
        {
            var lastVersion = await GetLastVersionAsync(entityId);

            if (lastVersion != expectedVersion)
                throw new ConcurrencyException(entityId, expectedVersion, lastVersion);

            var eventRecord = new EventRecord
            {
                Id = @event.Id,
                EntityId = entityId,
                EventType = @event.EventType,
                EventData = JsonSerializer.Serialize(@event, @event.GetType()),
                Version = lastVersion + 1,
                EventDate = @event.EventDate
            };

            _dbContext.Events.Add(eventRecord);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> GetLastVersionAsync(Guid entityId)
        {
            return await _dbContext.Events
                .Where(e => e.EntityId == entityId)
                .OrderByDescending(e => e.Version)
                .Select(e => e.Version)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<EventRecord>> GetEventsAsync(Guid entityId, int fromVersion = 0)
        {
            return await _dbContext.Events
                .Where(e => e.EntityId == entityId && e.Version >= fromVersion)
                .OrderBy(e => e.Version)
                .ToListAsync();
        }

        public async Task<IEnumerable<EventRecord>> GetAllEventsByEntityAsync(Guid entityId)
        {
            return await _dbContext.Events
                .AsNoTracking()
                .Where(e => e.EntityId == entityId)
                .OrderBy(e => e.Version)
                .ToListAsync();
        }
    }
}
