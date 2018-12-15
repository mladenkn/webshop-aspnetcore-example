namespace WebShop.Models
{
    public class BasketItemDiscounted
    {
        public int BasketItemId { get; set; }
        public int DiscountId { get; set; }

        public BasketItem BasketItem { get; set; }
        public Discount Discount { get; set; }
    }
}