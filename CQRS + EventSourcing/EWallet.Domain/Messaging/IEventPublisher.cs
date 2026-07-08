using EWallet.Domain.Entities;

namespace EWallet.Infrastructure.Messaging
{
    public interface IEventPublisher
    {
        Task PublishAsync(Guid entityId, DomainEvent @event);
    }
}
