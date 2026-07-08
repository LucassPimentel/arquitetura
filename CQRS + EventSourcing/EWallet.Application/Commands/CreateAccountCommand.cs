using EWallet.Domain.Entities;
using MediatR;

namespace EWallet.Application.Commands
{
    public class CreateAccountCommand : IRequest<Account>
    {
        public string AccountName { get; set; }
    }
}
