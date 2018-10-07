using System.Collections.Generic;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public class GrantedDiscount
    {
        public int DiscountId { get; set; }
        public int ItemId { get; set; }

        public Discount Discount { get; set; }
        public BasketItem Item { get; set; }
    }
}
