using AutoMapper;
using Order.Application.DTOs.Coupon;
using Order.Domain;

namespace Order.Application.Mappings
{
    public class CouponMappingProfile : Profile
    {
        public CouponMappingProfile()
        {
            CreateMap<Coupon, CouponDto>();
        }
    }
}
