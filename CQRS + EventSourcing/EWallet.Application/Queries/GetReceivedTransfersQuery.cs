using EWallet.Domain.Events;
using MediatR;

namespace EWallet.Application.Queries
{
    public class GetReceivedTransfersQuery : IRequest<IEnumerable<MoneyReceived>>
    {
        public Guid AccountId { get; set; }
    }
}
