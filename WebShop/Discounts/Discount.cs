namespace WebShop.Domain.Discounts
{
    public class Discount
    {
        public string Name { get; set; }
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
        public decimal Value { get; set; }
    }
}
