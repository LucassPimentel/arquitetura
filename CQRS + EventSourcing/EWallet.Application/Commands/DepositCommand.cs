using MediatR;

namespace EWallet.Application.Commands
{
    public class DepositCommand : IRequest<bool>
    {
        public Guid AccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
