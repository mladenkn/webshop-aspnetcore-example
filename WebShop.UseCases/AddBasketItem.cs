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
    public class AddBasketItem
    {
        public struct Request : IRequest
        {
            public int ProductId { get; set; }
            public int BasketId { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.ProductId).GreaterThan(0);
                RuleFor(r => r.BasketId).GreaterThan(0);
            }
        }
        
        public class Handler : RequestHandler<Request>
        {
            public Handler(NewTransaction newTransaction, 
                IMapper mapper,
                ApplyDiscountToBasketItem applyDiscount,
                IQueryable<Discount> discountStore)
            {
                HandleWith(async (request, cancellationToken) =>
                {
                    var basketItem = mapper.Map<BasketItem>(request);
                    await newTransaction().Save(basketItem).Commit();
                    var discounts = await GetDiscountsFor(basketItem);
                    await discounts.Select(d => applyDiscount(basketItem, d)).WhenAll();
                    return Responses.Success(basketItem);
                });

                async Task<IEnumerable<Discount>> GetDiscountsFor(BasketItem item)
                {
                    var numberOfProductsInBasket = item.Basket.Items.Count(i => i.ProductId == item.ProductId);
                    var discounts = await discountStore
                        .Where(d => d.ForProductId == item.ProductId && 
                                    numberOfProductsInBasket >= d.RequiredMinimalQuantity)
                        .ToListAsync();
                    return discounts;
                }
            }
        }
    }
}
