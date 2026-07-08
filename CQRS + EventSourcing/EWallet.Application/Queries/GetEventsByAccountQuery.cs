using EWallet.Domain.Entities;
using MediatR;

namespace EWallet.Application.Queries
{
    public class GetEventsByAccountQuery : IRequest<IEnumerable<EventRecord>>
    {
        public Guid AccountId { get; set; }
    }
}
