using AutoMapper;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Discount.MicroDiscount, MicroDiscount>()
                .ForMember(md => md.DiscountId, c => c.Ignore())
                .ReverseMap();

            CreateMap<Discount.RequiredProduct, RequiredProductOfDiscount>()
                .ForMember(md => md.DiscountId, c => c.Ignore())
                .ReverseMap();
        }
    }
}
