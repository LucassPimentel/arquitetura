using EWallet.Domain.Entities;
using EWallet.Infrastructure.Repositories;
using MediatR;

namespace EWallet.Application.Queries.Handlers
{
    public class GetAllAccountsHandler : IRequestHandler<GetAllAccountsQuery, IEnumerable<AccountReadModel>>
    {
        private readonly IAccountReadRepository _accountReadRepository;

        public GetAllAccountsHandler(IAccountReadRepository accountReadRepository)
        {
            _accountReadRepository = accountReadRepository;
        }

        public Task<IEnumerable<AccountReadModel>> Handle(GetAllAccountsQuery request, CancellationToken cancellationToken)
            => _accountReadRepository.GetAllAsync(cancellationToken);
    }
}
