using EWallet.Domain.Entities;

namespace EWallet.Infrastructure.Repositories
{
    public interface IAccountReadRepository
    {
        Task UpsertAsync(AccountReadModel account, CancellationToken cancellationToken = default);
        Task<AccountReadModel?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<IEnumerable<AccountReadModel>> GetAllAsync(CancellationToken cancellationToken = default);
    }
}
