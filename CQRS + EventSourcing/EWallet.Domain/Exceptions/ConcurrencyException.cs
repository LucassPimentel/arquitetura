namespace EWallet.Domain.Exceptions
{
    public class ConcurrencyException : Exception
    {
        public ConcurrencyException(Guid entityId, int expectedVersion, int actualVersion)
            : base($"Conflito de concorrência na entidade {entityId}. Versão esperada: {expectedVersion}, versão atual: {actualVersion}.")
        {
        }
    }
}
