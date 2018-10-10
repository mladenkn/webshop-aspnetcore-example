using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class BasketItemDiscount
    {
        public int BasketItemId { get; set; }
        public int DiscountId { get; set; }

        public BasketItem BasketItem { get; set; }
        public Discount Discount { get; set; }
    }
}
