namespace WebShop.Models
{
    public class DiscountedBasketItem
    {
        public int Id { get; set; }
        public int BasketItemId { get; set; }
        public int DiscountId { get; set; }
        public decimal NewPrice { get; set; }

        public BasketItem BasketItem { get; set; }
    }
}
