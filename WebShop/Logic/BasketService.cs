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
            var appliedDiscounts = discountsToBeApplied
                .Select(d => MapToAppliedDiscounts(basket, d))
                .SelectMany(ap => ap);
            basket.AppliedDiscounts = appliedDiscounts;
            RefreshBasketPrice(basket);
        }

        private void RefreshBasketPrice(Basket basket)
        {
            _mediator.Publish(new BasketPriceRequested(basket));
            // ....
        }

        private IEnumerable<AppliedDiscount> MapToAppliedDiscounts(Basket basket,
            Discount discount)
        {
            return discount.MicroDiscounts.Select(md => basket.Items
                .Where(bi => bi.ProductId == md.TargetProductId)
                .Take(md.MaxNumberOfTargetProductsToBeDiscounted)
                .Select(bi => new AppliedDiscount {BasketItemId = bi.Id, DiscountId = discount.Id, Value = md.Value})
            )
            .SelectMany(c => c);
        }
    }
}
