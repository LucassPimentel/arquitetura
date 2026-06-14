using AutoMapper;
using Order.Application.DTOs.Product;
using Order.Domain.Entities;

namespace Order.Application.Mappings
{
    public class ProductMappingProfile : Profile
    {
        public ProductMappingProfile()
        {
            CreateMap<Product, ProductDto>();
        }
    }
}
