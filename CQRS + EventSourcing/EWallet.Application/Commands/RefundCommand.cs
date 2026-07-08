using MediatR;

namespace EWallet.Application.Commands
{
    public class RefundCommand : IRequest<bool>
    {
        public Guid SourceAccountId { get; set; }
        public Guid DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public Guid OriginalTransferEventId { get; set; }
    }
}
