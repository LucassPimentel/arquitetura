using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Repositories;

namespace EWallet.Worker.Projections.Interfaces
{
    public interface IEventProjection
    {
        string EventType { get; }
        Task ProjectAsync(EventEnvelope envelope, IAccountReadRepository repository);
    }
}
