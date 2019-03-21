namespace WebShop.Models
{
    public class AppliedDiscount
    {
        public int Id { get; set; }
        public int BasketItemId { get; set; }
        public int DiscountId { get; set; }
        public decimal Value { get; set; }

        public BasketItem BasketItem { get; set; }
    }
}
