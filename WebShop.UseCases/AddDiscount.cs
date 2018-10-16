using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.Abstract;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Features
{
    public class AddDiscount
    {
        public struct Request : IRequest
        {
            public int ForProductId { get; set; }
            public int RequiredMinimalQuantity { get; set; }
            public decimal Value { get; set; }
            public int MaxNumberOfItemsToApplyTo { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.ForProductId).GreaterThan(0);
                RuleFor(r => r.RequiredMinimalQuantity).GreaterThan(0);
                RuleFor(r => r.Value).GreaterThan(0);
                RuleFor(r => r.MaxNumberOfItemsToApplyTo).GreaterThan(0);
            }
        }

        public class Handler : RequestHandler<Request>
        {
            public Handler(IMapper mapper,
                NewTransaction newTransaction, 
                ApplyDiscountToBasketItem applyDiscount,
                IQueryable<BasketItem> basketItemStore)
            {
                HandleWith(async (request, token) =>
                {
                    var discount = mapper.Map<Discount>(request);
                    await newTransaction().Save(discount).Commit();
                    var basketItems = await GetBasketItemsDiscountableWith(discount);
                    await basketItems.Select(basketItem => applyDiscount(basketItem, discount, newTransaction())).WhenAll();
                    return Responses.Success(discount);
                });

                async Task<IEnumerable<BasketItem>> GetBasketItemsDiscountableWith(Discount discount)
                {
                    var basketItems = await basketItemStore
                        .Where(i => i.Product.Id == discount.ForProductId &&
                                    i.Basket.Items.Count(bi => bi.ProductId == discount.ForProductId) >=
                                    discount.RequiredMinimalQuantity)
                        .Take(discount.MaxNumberOfItemsToApplyTo)
                        .ToListAsync();
                    return basketItems;
                }
            }
        }
    }
}
