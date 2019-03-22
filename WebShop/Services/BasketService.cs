using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Services
{
    public interface IBasketService
    {
        Task<BasketWithPrice> ApplyDiscounts(Basket basket);
    }

    public class BasketService : IBasketService
    {
        private readonly ISmartQueries _smartQueries;
        private readonly IMediator _mediator;

        public BasketService(ISmartQueries smartQueries, IMediator mediator)
        {
            _smartQueries = smartQueries;
            _mediator = mediator;
        }

        public async Task<BasketWithPrice> ApplyDiscounts(Basket basket)
        {
            basket.Items.Must().NotBeNull();
            var discountsToBeApplied = await _smartQueries.GetDiscountsFor(basket);

            var discountedBasketItems = discountsToBeApplied.Select(d =>
            {
                var basketItemsOfTargetProduct = basket.Items.Where(bi => bi.ProductId == d.TargetProductId);
                var countOfRequiredProducts = basket.Items.Count(bi => bi.ProductId == d.RequiredProductId);
                var numberOfBasketItemsToBeDiscounted = countOfRequiredProducts / d.RequiredPerOneDiscounted;

                return basketItemsOfTargetProduct
                    .Take(numberOfBasketItemsToBeDiscounted)
                    .Select(bi => new BasketWithPrice.Item
                    {
                        BasketItemId = bi.Id,
                        DiscountId = d.Id,
                        ProductId = bi.ProductId,
                        Price = bi.Product.RegularPrice - (bi.Product.RegularPrice * d.TargetProductDiscountedBy)
                    });
            })
            .SelectMany(i => i);

            var basketWithPrice = MapToDiscountedBasket(basket, discountedBasketItems);
            await _mediator.Publish(new BasketPriceCalculated(basketWithPrice));

            return basketWithPrice;
        }

        private BasketWithPrice MapToDiscountedBasket(Basket basket, IEnumerable<BasketWithPrice.Item> discountedBasketItems)
        {
            var notDiscountedBasketItems =
                basket.Items.Where(bi => !discountedBasketItems.Any(di => di.BasketItemId == bi.Id));

            BasketWithPrice.Item MapBasketItem(BasketItem bi) => new BasketWithPrice.Item
                {
                    BasketItemId = bi.Id, Price = bi.Product.RegularPrice, ProductId = bi.ProductId
                };

            var allItems = notDiscountedBasketItems.Select(MapBasketItem).Concat(discountedBasketItems);
            var price = allItems.Select(i => i.Price).Sum();

            return new BasketWithPrice
            {
                BasketId = basket.Id,
                Items = allItems,
                Price = price
            };
        }
    }
}
