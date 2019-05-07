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
        Task<BasketWithPrice> CalculatePrice(Basket basket);
        Task RemoveItem(int id);
    }

    public class BasketService : IBasketService
    {
        private readonly ISmartQueries _smartQueries;
        private readonly IMediator _mediator;
        private readonly IQueries _queries;
        private readonly IUnitOfWork _unitOfWork;

        public BasketService(ISmartQueries smartQueries, IMediator mediator, IQueries queries, IUnitOfWork unitOfWork)
        {
            _smartQueries = smartQueries;
            _mediator = mediator;
            _queries = queries;
            _unitOfWork = unitOfWork;
        }

        public async Task<BasketWithPrice> CalculatePrice(Basket basket)
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
            await _mediator.Publish(new BasketPriceRequested(basketWithPrice));

            return basketWithPrice;
        }

        public async Task RemoveItem(int id)
        {
            var item = await _queries.GetBasketItem(id);
            if(item == null)
                throw new ModelNotFoundException("Basket item not found.");
            _unitOfWork.Delete(item);
            await _unitOfWork.PersistChanges();
        }

        private BasketWithPrice MapToDiscountedBasket(Basket basket, IEnumerable<BasketWithPrice.Item> discountedBasketItems)
        {
            var notDiscountedBasketItems =
                basket.Items.Where(bi => !discountedBasketItems.Any(di => di.BasketItemId == bi.Id));

            var allItems = notDiscountedBasketItems
                .Select(bi => new BasketWithPrice.Item
                {
                    BasketItemId = bi.Id,
                    Price = bi.Product.RegularPrice,
                    ProductId = bi.ProductId
                })
                .Concat(discountedBasketItems);

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
