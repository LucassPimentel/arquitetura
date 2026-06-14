using Order.Domain.Entities;

namespace Order.Domain.Interfaces.Repositories
{
    public interface IProductRepository
    {
        Task AddAsync(Product product);
        Task<IEnumerable<Product>> GetAllAsync();
        Task<Product?> GetByNameAsync(string name);
        Task<Product?> GetByIdAsync(Guid id);
        void Update(Product product);
    }
}
