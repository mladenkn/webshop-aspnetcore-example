using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Logic
{
    public interface IDiscountService
    {
        Task<IReadOnlyCollection<BasketItemDiscounted>> ApplyDiscounts(Basket basket);
    }

    public class DiscountService : IDiscountService
    {
        private readonly ISmartQueries _smartQueries;

        public DiscountService(ISmartQueries smartQueries)
        {
            _smartQueries = smartQueries;
        }

        public async Task<IReadOnlyCollection<BasketItemDiscounted>> ApplyDiscounts(Basket basket)
        {
            basket.Items.Must().NotBeNull();

            var discounts = await _smartQueries.GetDiscountsFor(basket);
            var appliedDiscounts = discounts.Select(d =>
                {
                    var discountedBasketItems = basket.Items
                        .Where(bi => bi.ProductId == d.TargetProductId)
                        .Take(d.TargetProductQuantity);

                    var appliedDiscounts_ = discountedBasketItems
                        .Select(bi => new BasketItemDiscounted
                        {
                            BasketItemId = bi.Id,
                            DiscountId = d.Id,
                            BasketItem = bi,
                            Discount = d
                        });

                    return appliedDiscounts_;
                })
                .SelectMany(ad => ad)
                .ToList();
            return appliedDiscounts;
        }
    }
}
