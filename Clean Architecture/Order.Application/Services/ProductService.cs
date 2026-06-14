using AutoMapper;
using Order.Application.DTOs.Product;
using Order.Application.Interfaces;
using Order.Domain.Entities;
using Order.Domain.Errors;
using Order.Domain.Interfaces.Repositories;

namespace Order.Application.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<Result<ProductDto>> CreateAsync(CreateProductDto dto)
        {
            var existingProduct = await _repository.GetByNameAsync(dto.Name);
            if (existingProduct != null)
                return Result<ProductDto>.Failure(ProductErrors.AlreadyExists);

            var result = Product.Create(dto.Name, dto.Price);

            if (!result.IsSuccess)
                return Result<ProductDto>.Failure(result.Error!);

            await _repository.AddAsync(result.Value!);
            return Result<ProductDto>.Success(_mapper.Map<ProductDto>(result.Value!));
        }

        public async Task<Result<ProductDto>> UpdateAsync(UpdateProductDto dto)
        {
            var result = await _repository.GetByIdAsync(dto.Id);

            if (result is null)
                return Result<ProductDto>.Failure(ProductErrors.NonExistent);

            result.Update(dto.Name, dto.Price, dto.Active);

            _repository.Update(result);
            return Result<ProductDto>.Success(_mapper.Map<ProductDto>(result));
        }

        public async Task<IEnumerable<ProductDto>> GetAllAsync()
        {
            var products = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<ProductDto>>(products);
        }
    }
}
