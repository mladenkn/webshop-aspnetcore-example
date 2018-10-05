using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class BasketService : IBasketService
    {
        private readonly IMediator _mediator;
        private readonly IQueryable<Discount> _discounts;

        public BasketService(IMediator mediator, IQueryable<Discount> discounts)
        {
            _mediator = mediator;
            _discounts = discounts;
        }

        public async Task<IEnumerable<GrantedDiscount>> GrantDiscounts(Basket basket)
        {
            var discountsToGrant = _discounts
                .Where(discount => basket.Items.Any(basketItem => basketItem.ProductId == discount.ProductId) && 
                                   basket.Items.Count >= discount.RequiredQuantity)
                .ToList();

            basket.GrantedDiscounts = discountsToGrant.Select(d =>
            {
                var items = basket.Items
                    .Where(i => i.ProductId == d.ProductId)
                    .Take(d.MaxNumberOfItemsToApplyTo)
                    .ToList();
                return new GrantedDiscount(d, items);
            })
            .ToList();
            
            basket.GrantedDiscounts
                .Select(d => new DiscountGrantedNotification(d))
                .ForEach(n => _mediator.Publish(n));

            return basket.GrantedDiscounts;
        }
    }

    public interface IBasketService
    {
        Task<IEnumerable<GrantedDiscount>> GrantDiscounts(Basket basket);
    }
}
