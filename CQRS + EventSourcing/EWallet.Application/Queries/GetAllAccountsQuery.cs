using EWallet.Domain.Entities;
using MediatR;

namespace EWallet.Application.Queries
{
    public class GetAllAccountsQuery : IRequest<IEnumerable<AccountReadModel>>
    {
    }
}
