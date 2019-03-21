using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using WebShop.Models;

namespace WebShop.Infrastructure.DataAccess
{
    //public interface ICustomMapper
    //{
    //    (IEnumerable<RequiredProductOfDiscount> requiredProducts, IEnumerable<MicroDiscount> microDiscounts) ToDbModels(Discount domainModel);
    //    Discount ToDomainModel(IReadOnlyCollection<Discount.RequiredProduct> requiredProducts, IReadOnlyCollection<Discount.MicroDiscount> microDiscounts);
    //}

    //public class CustomMapper : ICustomMapper
    //{
    //    private readonly IMapper _mapper;

    //    public CustomMapper(IMapper mapper)
    //    {
    //        _mapper = mapper;
    //    }

    //    public Discount ToDomainModel(IReadOnlyCollection<Discount.RequiredProduct> requiredProducts,
    //        IReadOnlyCollection<Discount.MicroDiscount> microDiscounts)
    //    {
    //        var id = requiredProducts.First().DiscountId;
    //        return new Discount(id, requiredProducts, microDiscounts);
    //    }

    //    public (IEnumerable<RequiredProductOfDiscount> requiredProducts,
    //        IEnumerable<MicroDiscount> microDiscounts) ToDbModels(Discount domainModel)
    //    {
    //        var requiredProducts2 = domainModel.RequiredProducts.Select(m =>
    //        {
    //            var m2 = _mapper.Map<RequiredProductOfDiscount>(m);
    //            m2.DiscountId = domainModel.Id;
    //            return m2;
    //        });

    //        var microDiscounts2 = domainModel.MicroDiscounts.Select(m =>
    //        {
    //            var m2 = _mapper.Map<MicroDiscount>(m);
    //            m2.DiscountId = domainModel.Id;
    //            return m2;
    //        });

    //        return (requiredProducts2, microDiscounts2);
    //    }
    //}
}
