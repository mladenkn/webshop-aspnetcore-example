using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Logic
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

            var itemsWithDiscounts = discountsToBeApplied.Select(d =>
                {
                    var basketItemsOfTargetProduct = basket.Items.Where(bi => bi.ProductId == d.TargetProductId);
                    var countOfRequiredProducts = basket.Items.Count(bi => bi.ProductId == d.RequiredProductId);
                    var numberOfBasketItemsToBeDiscounted = countOfRequiredProducts / d.RequiredPerOneDiscounted;

                    return basketItemsOfTargetProduct
                        .Take(numberOfBasketItemsToBeDiscounted)
                        .Select(bi => new DiscountedBasketItem
                        {
                            BasketItemId = bi.Id,
                            BasketItem = bi,
                            DiscountId = d.Id,
                            NewPrice = bi.Product.RegularPrice - (bi.Product.RegularPrice * d.TargetProductDiscountedBy)
                        });
                })
                .SelectMany(i => i);

            return MapToDiscountedBasket(basket, itemsWithDiscounts);
        }

        private BasketWithPrice MapToDiscountedBasket(Basket basket, IEnumerable<DiscountedBasketItem> discountedBasketItems)
        {
            _mediator.Publish(new BasketPriceRequested(basket));
            var price = discountedBasketItems.Select(i => i.NewPrice).Sum();
            return new BasketWithPrice
            {
                BasketId = basket.Id,
                BasketItems = basket.Items,
                DiscountedItems = discountedBasketItems,
                Price = price
            };
        }

        //private IEnumerable<AppliedDiscount> MapToAppliedDiscounts(
        //    Basket basket, IEnumerable<BasketDiscountSpecification.BasketItemDiscount> discounts)
        //{
        //    return discounts.Select(md => basket.Items
        //        .Where(bi => bi.ProductId == md.TargetProductId)
        //        .Take(md.MaxNumberOfTargetProductsToBeDiscounted)
        //        .Select(bi => new AppliedDiscount
        //        {
        //            BasketItemId = bi.Id, DiscountId = discounts., Value = md.Value, BasketItem = bi
        //        })
        //    )
        //    .SelectMany(ap => ap);
        //}
    }
}
