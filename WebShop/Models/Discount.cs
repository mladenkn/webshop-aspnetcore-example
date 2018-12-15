namespace WebShop.Models
{
    public class Discount
    {
        public int Id { get; set; }

        public int RequiredProductId { get; set; }
        public Product RequiredProduct { get; set; }
        public int RequiredProductRequiredQuantity { get; set; }

        public int TargetProductId { get; set; }
        public Product TargetProduct { get; set; }
        public int TargetProductQuantity { get; set; }

        public decimal Value { get; set; }
    }
}
