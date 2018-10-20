using System.Collections.Generic;
using System.Linq;
using Utilities;
using WebShop.BasketItems;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public delegate bool ShouldApplyToBasketItem(
        BasketItem basketItem, Discount discount, ICollection<DiscountGranted> grantedDiscounts);

    public class DiscountService
    {
        public static bool ShouldApplyToBasketItem(
            BasketItem basketItem, Discount discount, ICollection<DiscountGranted> grantedDiscounts)
        {
            if (discount.TargetProductId != basketItem.ProductId)
                return false;

            var numOfTimesToGrant =
                basketItem.Basket.Items.Count(it => it.ProductId == discount.RequiredProductId) /
                discount.RequiredProductQuantity;

            var isGrantedMaxTimes = grantedDiscounts
                .ContainsN(it => it.ProductId == basketItem.ProductId && it.DiscountId == discount.Id, numOfTimesToGrant);

            var shouldGrant = !isGrantedMaxTimes;

            if (shouldGrant)
                grantedDiscounts.Add(new DiscountGranted(basketItem.ProductId, discount.Id));

            return shouldGrant;
        }
    }
}
