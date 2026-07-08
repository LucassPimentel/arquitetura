namespace EWallet.Domain.Entities
{
    /// <summary>
    /// View materializada da conta, otimizada para leitura (CQRS read side).
    /// </summary>
    public class AccountReadModel
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
