using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<GrantedDiscount> GrantDiscounts(Basket basket)
        {
            var discountsToGrant = _discounts
                .Where(discount => basket.Items.Any(basketItem => basketItem.ProductId == discount.ProductId &&
                                                                  basketItem.Quantity >= discount.RequiredQuantity))
                .ToList();

            Product GetProduct(int id) => basket.Items.First(p => p.ProductId == id).Product;

            basket.GrantedDiscounts = discountsToGrant.Select(d => new GrantedDiscount(d, GetProduct(d.ProductId))).ToList();
            
            basket.GrantedDiscounts
                .Select(d => new DiscountGrantedNotification(d))
                .ForEach(n => _mediator.Publish(n));

            return basket.GrantedDiscounts;
        }
    }

    public interface IBasketService
    {
        IEnumerable<GrantedDiscount> GrantDiscounts(Basket basket);
    }
}
