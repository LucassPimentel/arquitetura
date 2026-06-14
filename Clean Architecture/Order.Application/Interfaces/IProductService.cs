using Order.Application.DTOs.Product;
using Order.Domain.Errors;

namespace Order.Application.Interfaces
{
    public interface IProductService
    {
        Task<Result<ProductDto>> CreateAsync(CreateProductDto dto);
        Task<Result<ProductDto>> UpdateAsync(UpdateProductDto dto);
        Task<IEnumerable<ProductDto>> GetAllAsync();
    }
}
