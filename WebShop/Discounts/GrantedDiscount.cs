using System.Collections.Generic;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public class GrantedDiscount
    {
        public GrantedDiscount(Discount discount, IReadOnlyCollection<BasketItem> items)
        {
            Discount = discount;
            Items = items;
        }

        public Discount Discount { get; }

        public IReadOnlyCollection<BasketItem> Items { get; }
    }
}
