using System.Collections.Generic;
using System.Linq;
using WebShop.Domain.Baskets;

namespace WebShop.Domain.Discounts
{
    public class DiscountService
    {
        public IEnumerable<GrantedDiscount> ApplyDiscounts(Basket basket, IEnumerable<Discount> discounts)
        {
            var discountsToGrant = discounts
                .Where(discount => basket.Items.Any(basketItem => basketItem.ProductId == discount.ProductId &&
                                                                  basketItem.Quantity >= discount.RequiredQuantity));
            Product GetProduct(int id) => basket.Items.First(p => p.ProductId == id).Product;
            basket.GrantedDiscounts = discountsToGrant.Select(d => new GrantedDiscount(d, GetProduct(d.ProductId))).ToList();
            return basket.GrantedDiscounts;
        }
    }
}
