using System;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Abstract;

namespace WebShop.Baskets
{
    public class BasketItemService : IAsyncModelInitializer<BasketItem>
    {
        public async Task Initialize(BasketItem item)
        {
            // TODO: get item.Basket from db if null
            item.Price = GetItemPrice(item);
        }

        private static decimal GetItemPrice(BasketItem item)
        {
            // item can have multiple discounts
            var itemDiscounts = item.Basket.GrantedDiscounts
                .Where(d => d.ItemId == item.Id)
                .Select(d => d.Discount);

            var regularPrice = item.Product.RegularPrice;

            var totalDiscount = itemDiscounts.Sum(d => d.Value);
            if (totalDiscount > 1) // ensure discount is 100% max
                totalDiscount = 1;

            var discountedPrice = regularPrice * totalDiscount;

            return regularPrice - discountedPrice;
        }
    }
}
