using System.Linq;
using ApplicationKernel.Domain.MediatorSystem;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Utilities;
using WebShop.Baskets;

namespace WebShop.UseCases
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
            public Handler(IQueryable<Basket> baskets, decimal maxAllowedDiscount)
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
                    basket.Items.ForEach(CalculateItemPrice);

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
            }
        }
    }
}
