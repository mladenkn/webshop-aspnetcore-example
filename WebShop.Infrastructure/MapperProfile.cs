using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Discount.MicroDiscount, MicroDiscount>().ReverseMap();
            CreateMap<Discount.RequiredProduct, RequiredProductOfDiscount>().ReverseMap();
        }
    }
}
