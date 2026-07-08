using EWallet.Domain.Entities;
using MediatR;

namespace EWallet.Application.Queries
{
    public class GetAccountByIdQuery : IRequest<AccountReadModel?>
    {
        public Guid AccountId { get; set; }
    }
}
