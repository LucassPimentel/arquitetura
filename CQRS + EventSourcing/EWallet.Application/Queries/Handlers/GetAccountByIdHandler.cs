using EWallet.Domain.Entities;
using EWallet.Infrastructure.Repositories;
using MediatR;

namespace EWallet.Application.Queries.Handlers
{
    public class GetAccountByIdHandler : IRequestHandler<GetAccountByIdQuery, AccountReadModel?>
    {
        private readonly IAccountReadRepository _accountReadRepository;

        public GetAccountByIdHandler(IAccountReadRepository accountReadRepository)
        {
            _accountReadRepository = accountReadRepository;
        }

        public Task<AccountReadModel?> Handle(GetAccountByIdQuery request, CancellationToken cancellationToken)
            => _accountReadRepository.GetByIdAsync(request.AccountId, cancellationToken);
    }
}
