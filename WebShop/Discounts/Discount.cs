namespace WebShop.Discounts
{
    public class Discount
    {
        public int Id { get; set; }
        public int ForProductId { get; set; }
        public int RequiredProductId { get; set; }
        public int RequiredProductMinimalQuantity { get; set; }
        public decimal Value { get; set; }
        public int MaxNumberOfItemsToApplyTo { get; set; }

        public Product ForProduct { get; set; }
        public Product RequiredProduct { get; set; }
    }
}
