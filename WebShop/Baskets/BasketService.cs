using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using WebShop.Abstract;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class BasketService : IAsyncModelInitializer<Basket>
    {
        private readonly IMediator _mediator;
        private readonly IQueryable<Discount> _discounts;
        private readonly IAsyncModelInitializer<BasketItem> _basketItemInitializer;

        public BasketService(
            IMediator mediator,
            IQueryable<Discount> discounts,
            IAsyncModelInitializer<BasketItem> basketItemInitializer)
        {
            _mediator = mediator;
            _discounts = discounts;
            _basketItemInitializer = basketItemInitializer;
        }

        public async Task<IEnumerable<GrantedDiscount>> GrantDiscounts(Basket basket)
        {
            // not using async to simplify tests
            var discountsToGrant = _discounts
                .Where(discount => basket.Items.Any(basketItem => basketItem.ProductId == discount.ProductId) && 
                                   basket.Items.Count >= discount.RequiredMinimalQuantity)
                .ToList();

            var grantedDiscounts = discountsToGrant.SelectMany(discount =>
            {
                var items = basket.Items
                    .Where(i => i.ProductId == discount.ProductId)
                    .Take(discount.MaxNumberOfItemsToApplyTo)
                    .ToList();
                return items.Select(i => new GrantedDiscount
                {
                    Discount = discount,
                    Item = i,
                    DiscountId = discount.Id,
                    ItemId = i.Id
                });
            });

            grantedDiscounts
                .Select(d => new DiscountGrantedNotification(d))
                .ForEach(n => _mediator.Publish(n));

            return grantedDiscounts;
        }

        public async Task Initialize(Basket basket)
        {
            basket.GrantedDiscounts = (await GrantDiscounts(basket)).ToList();
            var initDiscountTasks = basket.GrantedDiscounts.Select(d => d.Item).Select(_basketItemInitializer.Initialize);
            await Task.WhenAll(initDiscountTasks);
            basket.TotalPrice = basket.Items.Select(i => i.Price).Sum();
        }
    }
}
