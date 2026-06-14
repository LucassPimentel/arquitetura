using Microsoft.EntityFrameworkCore;
using Order.Domain.Interfaces.Repositories;
using Order.Infrastructure.Context;

namespace Order.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Domain.Entities.Order order)
        {
            await _context.Orders.AddAsync(order);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Domain.Entities.Order>> GetAllAsync()
        {
            return await _context.Orders
                .Include(o => o.Items)
                .ToListAsync();
        }

        public async Task<Domain.Entities.Order?> GetByIdAsync(Guid id)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == id);
        }

        public void Update(Domain.Entities.Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }
    }
}
