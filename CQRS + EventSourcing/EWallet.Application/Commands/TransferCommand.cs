using MediatR;

namespace EWallet.Application.Commands
{
    public class TransferCommand : IRequest<bool>
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
    }
}
