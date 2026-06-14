using AutoMapper;
using Order.Application.DTOs.Order;
using Order.Application.Interfaces;
using Order.Domain.Errors;
using Order.Domain.Interfaces.Repositories;

namespace Order.Application.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;
        private readonly IProductRepository _productRepository;
        private readonly ICouponRepository _couponRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository repository, IProductRepository productRepository, ICouponRepository couponRepository, IMapper mapper)
        {
            _repository = repository;
            _productRepository = productRepository;
            _couponRepository = couponRepository;
            _mapper = mapper;
        }

        public async Task<Result<OrderDto>> CreateAsync(CreateOrderDto dto)
        {
            var items = new List<Domain.Entities.OrderItem>();

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    return Result<OrderDto>.Failure(ProductErrors.NonExistent);

                items.Add(new Domain.Entities.OrderItem(product, itemDto.Quantity));
            }

            var result = Domain.Entities.Order.Create(dto.ClientId, items);

            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            await _repository.AddAsync(result.Value!);

            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(result.Value!));
        }

        public async Task<IEnumerable<OrderDto>> GetAllAsync()
        {
            var orders = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<OrderDto>>(orders);
        }

        public async Task<Result<OrderDto>> UpdateAsync(UpdateOrderDto dto)
        {
            var order = await _repository.GetByIdAsync(dto.Id);

            if (order == null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            var items = new List<Domain.Entities.OrderItem>();

            foreach (var itemDto in dto.Items)
            {
                var product = await _productRepository.GetByIdAsync(itemDto.ProductId);
                if (product == null)
                    return Result<OrderDto>.Failure(ProductErrors.NonExistent);

                items.Add(new Domain.Entities.OrderItem(product, itemDto.Quantity));
            }

            var result = order.Update(items);

            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            _repository.Update(order);

            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> GetByIdAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> AddItemAsync(Guid orderId, Guid productId, int quantity)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order is null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            var product = await _productRepository.GetByIdAsync(productId);
            if (product is null)
                return Result<OrderDto>.Failure(ProductErrors.NonExistent);

            var item = new Domain.Entities.OrderItem(product, quantity);

            var result = order.AddItem(item);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            _repository.Update(order);
            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> RemoveItemAsync(Guid orderId, Guid itemId)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order is null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            var result = order.RemoveItem(itemId);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            _repository.Update(order);
            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> CloseOrderAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            var result = order.CloseOrder();

            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            _repository.Update(order);

            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> CancelOrderAsync(Guid id)
        {
            var order = await _repository.GetByIdAsync(id);

            if (order == null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            order.CancelOrder();

            _repository.Update(order);

            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }

        public async Task<Result<OrderDto>> ApplyCouponAsync(Guid orderId, string couponCode)
        {
            var order = await _repository.GetByIdAsync(orderId);
            if (order is null)
                return Result<OrderDto>.Failure(OrderErrors.NonExistent);

            var coupon = await _couponRepository.GetByCodeAsync(couponCode);
            if (coupon is null)
                return Result<OrderDto>.Failure(CouponErrors.NonExistent);

            var result = order.ApplyCoupon(coupon);
            if (!result.IsSuccess)
                return Result<OrderDto>.Failure(result.Error!);

            _repository.Update(order);
            return Result<OrderDto>.Success(_mapper.Map<OrderDto>(order));
        }
    }
}
