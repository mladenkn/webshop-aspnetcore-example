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
        Task ApplyDiscounts(Basket basket);
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

        public async Task ApplyDiscounts(Basket basket)
        {
            basket.Items.Must().NotBeNull();
            var discountsToBeApplied = await _smartQueries.GetDiscountsFor(basket);
            
            //var appliedDiscounts = discountsToBeApplied
            //    .Select(basketDiscount => MapToAppliedDiscounts(basket, basketDiscount.BasketItemDiscounts))
            //    .SelectMany(ap => ap);
            //basket.AppliedDiscounts = appliedDiscounts;
         
            discountsToBeApplied.Select(d =>
            {
                var numberOfBasketItemsThatShouldReceiveIt = d.RequiredProducts
            })

            RefreshBasketPrice(basket);
        }

        private void RefreshBasketPrice(Basket basket)
        {
            _mediator.Publish(new BasketPriceRequested(basket));
            
        }

        private void Apply(Basket basket, BasketDiscount.BasketItemDiscount discount, int times)
        {

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
