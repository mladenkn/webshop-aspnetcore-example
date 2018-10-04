namespace WebShop.Discounts
{
    public class Discount
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
        public decimal Value { get; set; }
        public int MaxNumberOfItemsToApplyTo { get; set; }
    }
}
