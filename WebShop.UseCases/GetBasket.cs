using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using WebShop.Baskets;
using WebShop.Discounts;

namespace WebShop.Features
{
    public class GetBasket
    {
        public struct Request : IRequest
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(r => r.Id).GreaterThan(0);
            }
        }

        public class Handler : RequestHandler<Request>
        {
            public Handler(IQueryable<Basket> baskets, IQueryable<Discount> discountStore, decimal maxAllowedDiscount)
            {
                HandleWith(async (request, token) =>
                {
                    var basket = await baskets
                        .Where(b => b.Id == request.Id)
                        .Include(b => b.Items)
                            .ThenInclude(i => i.Product)
                        .Include(b => b.Items)
                            .ThenInclude(b => b.Discounts)
                        .FirstOrDefaultAsync();

                    if (basket == null)
                        return Responses.Failure("Basket not found");

                    basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();
                    
                    foreach (var basketItem in basket.Items)
                    {
                        basketItem.Discounts = await GetDiscountsFor(basketItem);
                        CalculateItemPrice(basketItem);
                    }

                    return Responses.Success(basket);
                });

                void CalculateItemPrice(BasketItem item)
                {
                    var totalDiscount = item.Discounts.Select(d => d.Value).Sum();
                    if (totalDiscount > maxAllowedDiscount)
                        totalDiscount = maxAllowedDiscount;

                    var without = item.Product.RegularPrice * totalDiscount;
                    item.Price = item.Product.RegularPrice - without;
                }

                async Task<List<Discount>> GetDiscountsFor(BasketItem item)
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
