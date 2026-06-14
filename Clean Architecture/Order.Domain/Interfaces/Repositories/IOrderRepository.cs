namespace Order.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Entities.Order order);
        Task<IEnumerable<Entities.Order>> GetAllAsync();
        Task<Entities.Order?> GetByIdAsync(Guid id);
        void Update(Entities.Order order);
    }
}
