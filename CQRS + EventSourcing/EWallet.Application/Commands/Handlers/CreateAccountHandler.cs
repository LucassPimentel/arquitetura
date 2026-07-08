using EWallet.Application.Events;
using EWallet.Domain.Entities;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Persistance.Event_Store;
using MediatR;

namespace EWallet.Application.Commands.Handlers
{
    public class CreateAccountHandler : IRequestHandler<CreateAccountCommand, Account>
    {
        private readonly IEventStoreRepository _eventStore;
        private readonly IEventPublisher _eventPublisher;

        public CreateAccountHandler(IEventStoreRepository eventStore, IEventPublisher eventPublisher)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
        }

        public async Task<Account> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var newAccount = Account.CreateAccount(request.AccountName);

            var accountCreated = new AccountCreated(newAccount.Id, request.AccountName);

            // Conta nova: nenhum evento existe ainda, versão esperada é 0
            await _eventStore.SaveEventAsync(newAccount.Id, accountCreated, expectedVersion: 0);
            await _eventPublisher.PublishAsync(newAccount.Id, accountCreated);

            return newAccount;
        }
    }
}
