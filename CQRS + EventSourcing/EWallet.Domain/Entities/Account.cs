using EWallet.Application.Events;
using EWallet.Domain.Enums;
using EWallet.Domain.Events;
using EWallet.Domain.Helpers;
using System.Text.Json.Serialization;

namespace EWallet.Domain.Entities
{
    public class Account 
    {
        public Guid Id { get; private set; }
        public decimal Balance { get; private set; }
        public Status Status { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string UserName { get; private set; }
        public int Version { get; private set; }

        public IReadOnlyList<MoneyReceived> TransferenciasRecebidas => _transferenciasRecebidas;
        private readonly List<MoneyReceived> _transferenciasRecebidas = new();
        private readonly HashSet<Guid> _eventosReembolsados = new();

        [JsonConstructor]
        private Account() { }

        public static Account CreateAccount(string userName)
        {
            return new Account
            {
                Id = Guid.NewGuid(),
                Balance = 0,
                Status = Status.Active,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                UserName = userName
            };
        }

        public void Deposit(Account account, decimal amount)
        {
            if (account.Status != Status.Active)
                throw new InvalidOperationException("Não é possível depositar em uma conta não ativa.");

            if (amount <= 0)
                throw new ArgumentException("O valor do depósito deve ser maior que zero.");

            Balance += amount;
            UpdatedAt = DateTime.Now;
        }

        public void Transfer(Account destinationAccount, decimal amount)
        {
            if (Status != Status.Active)
                throw new InvalidOperationException("Não é possível transferir de uma conta não ativa.");

            if (destinationAccount.Status != Status.Active)
                throw new InvalidOperationException("Não é possível transferir para uma conta não ativa.");

            if (amount <= 0)
                throw new ArgumentException("O valor da transferência deve ser maior que zero.");

            if (Balance < amount)
                throw new InvalidOperationException("Saldo insuficiente para a transferência.");

            Balance -= amount;
            destinationAccount.Balance += amount;
            UpdatedAt = DateTime.Now;
            destinationAccount.UpdatedAt = DateTime.Now;
        }

        public void Refund(Account destinationAccount, decimal amount, Guid originalTransferEventId)
        {
            if (Status != Status.Active)
                throw new InvalidOperationException("Não é possível reembolsar de uma conta não ativa.");

            if (destinationAccount.Status != Status.Active)
                throw new InvalidOperationException("Não é possível reembolsar para uma conta não ativa.");

            if (amount <= 0)
                throw new ArgumentException("O valor do reembolso deve ser maior que zero.");

            var transferenciaOriginal = _transferenciasRecebidas
                .FirstOrDefault(t => t.Id == originalTransferEventId);

            if (transferenciaOriginal is null)
                throw new InvalidOperationException("Transferência não encontrada no histórico desta conta.");

            if (transferenciaOriginal.SourceAccountId != destinationAccount.Id)
                throw new InvalidOperationException("A conta de destino do reembolso não corresponde à conta de origem da transferência.");

            if (amount > transferenciaOriginal.Amount)
                throw new InvalidOperationException($"O valor do reembolso ({amount:C}) não pode ser maior que o valor recebido ({transferenciaOriginal.Amount:C}).");

            if (amount != transferenciaOriginal.Amount)
                throw new InvalidOperationException($"O reembolso deve ser pelo valor total recebido ({transferenciaOriginal.Amount:C}).");

            if (_eventosReembolsados.Contains(originalTransferEventId))
                throw new InvalidOperationException("Esta transferência já foi reembolsada.");

            if (Balance < amount)
                throw new InvalidOperationException("Saldo insuficiente para o reembolso.");

            Balance -= amount;
            destinationAccount.Balance += amount;
            UpdatedAt = DateTime.Now;
            destinationAccount.UpdatedAt = DateTime.Now;
        }

        public void BlockAccount()
        {
            if (Status == Status.Blocked)
                throw new InvalidOperationException("A conta já está bloqueada.");
            Status = Status.Blocked;
            UpdatedAt = DateTime.Now;
        }

        public static Account Rehydrate(IEnumerable<EventRecord> records)
        {
            var account = new Account();

            foreach (var record in records)
            {
                var @event = EventDeserializer.Deserialize(record);
                account.Apply(@event);
                account.Version = record.Version;
            }

            return account;
        }
        private void Apply(DomainEvent @event)
        {
            switch (@event)
            {
                case AccountCreated e:
                    Id = e.EntityId;
                    UserName = e.AccountName;
                    Balance = 0;
                    Status = Status.Active;
                    CreatedAt = e.EventDate;
                    UpdatedAt = e.EventDate;
                    break;

                case MoneyDeposited e:
                    Balance = e.NewBalance;
                    UpdatedAt = e.EventDate;
                    break;

                case MoneyTransferred e:
                    Balance -= e.Amount;
                    UpdatedAt = e.EventDate;
                    break;

                case MoneyReceived e:
                    Balance += e.Amount;
                    _transferenciasRecebidas.Add(e);
                    UpdatedAt = e.EventDate;
                    break;

                case MoneyRefunded e:
                    Balance -= e.Amount;
                    _eventosReembolsados.Add(e.OriginalTransferEventId);
                    UpdatedAt = e.EventDate;
                    break;

                case MoneyRefundReceived e:
                    Balance += e.Amount;
                    UpdatedAt = e.EventDate;
                    break;

                case AccountBlocked:
                    Status = Status.Blocked;
                    UpdatedAt = @event.EventDate;
                    break;
            }
        }
    }
}