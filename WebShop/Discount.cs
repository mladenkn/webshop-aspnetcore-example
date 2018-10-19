namespace WebShop
{
    public class Discount
    {
        public int Id { get; set; }
        public int TargetProductId { get; set; }
        public int RequiredProductId { get; set; }
        public int TargetProductQuantity { get; set; }
        public int RequiredProductQuantity { get; set; }
        public decimal Value { get; set; }

        public Product TargetProduct { get; set; }
        public Product RequiredProduct { get; set; }
    }
}
