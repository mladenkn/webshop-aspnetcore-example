namespace WebShop.Infrastructure.DataAccess
{
    public class RequiredProductOfDiscount
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public int RequiredQuantity { get; set; }
    }

    public class MicroDiscount
    {
        public int DiscountId { get; set; }
        public int TargetProductId { get; set; }
        public int MaxNumberOfTargetProductsToBeDiscounted { get; set; }
        public decimal Value { get; set; }
    }
}
