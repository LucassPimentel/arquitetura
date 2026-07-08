using EWallet.Domain.Entities;
using EWallet.Domain.Events;
using EWallet.Infrastructure.Messaging;
using EWallet.Infrastructure.Persistance.Event_Store;
using EWallet.Infrastructure.Repositories;
using MediatR;

namespace EWallet.Application.Commands.Handlers
{
    public class DepositHandler : IRequestHandler<DepositCommand, bool>
    {
        private readonly IEventStoreRepository _eventStore;
        private readonly IEventPublisher _eventPublisher;
        private readonly IAccountReadRepository _accountReadRepository;

        public DepositHandler(
            IEventStoreRepository eventStore,
            IEventPublisher eventPublisher,
            IAccountReadRepository accountReadRepository)
        {
            _eventStore = eventStore;
            _eventPublisher = eventPublisher;
            _accountReadRepository = accountReadRepository;
        }

        public async Task<bool> Handle(DepositCommand request, CancellationToken cancellationToken)
        {
            var records = await _eventStore.GetEventsAsync(request.AccountId, 0);
            if (!records.Any())
                throw new InvalidOperationException($"Conta {request.AccountId} não encontrada.");

            var account = Account.Rehydrate(records);

            account.Deposit(account, request.Amount);

            var depositEvent = new MoneyDeposited(request.AccountId, request.Amount, account.Balance);

            await _eventStore.SaveEventAsync(request.AccountId, depositEvent, account.Version);
            await _eventPublisher.PublishAsync(request.AccountId, depositEvent);

            return true;
        }
    }
}
