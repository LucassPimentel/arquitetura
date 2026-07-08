using MediatR;

namespace EWallet.Application.Commands
{
    public class BlockAccountCommand : IRequest<bool>
    {
        public Guid AccountId { get; set; }
    }
}
