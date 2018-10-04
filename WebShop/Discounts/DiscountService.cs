using System.Collections.Generic;
using System.Linq;
using MediatR;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public class DiscountService
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IEnumerable<GrantedDiscount> ApplyDiscounts(Basket basket, IEnumerable<Discount> discounts)
        {
            var discountsToGrant = discounts
                .Where(discount => basket.Items.Any(basketItem => basketItem.ProductId == discount.ProductId &&
                                                                  basketItem.Quantity >= discount.RequiredQuantity));

            Product GetProduct(int id) => basket.Items.First(p => p.ProductId == id).Product;

            basket.GrantedDiscounts = discountsToGrant.Select(d => new GrantedDiscount(d, GetProduct(d.ProductId))).ToList();
            
            basket.GrantedDiscounts
                .Select(d => new DiscountGrantedNotification(d))
                .ForEach(n => _mediator.Publish(n));

            return basket.GrantedDiscounts;
        }
    }
}
