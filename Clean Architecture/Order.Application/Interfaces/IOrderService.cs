using Order.Application.DTOs.Order;
using Order.Domain.Errors;

namespace Order.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto);
        Task<Result<OrderDto>> UpdateAsync(UpdateOrderDto dto);
        Task<Result<OrderDto>> AddItemAsync(Guid orderId, Guid productId, int quantity);
        Task<Result<OrderDto>> RemoveItemAsync(Guid orderId, Guid itemId);
        Task<IEnumerable<OrderDto>> GetAllAsync();
        Task<Result<OrderDto>> GetByIdAsync(Guid id);
        Task<Result<OrderDto>> CloseOrderAsync(Guid id);
        Task<Result<OrderDto>> CancelOrderAsync(Guid id);
        Task<Result<OrderDto>> ApplyCouponAsync(Guid orderId, string couponCode);
    }
}
