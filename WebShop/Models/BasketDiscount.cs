using Utilities;

namespace WebShop.Models
{
    public class BasketDiscount
    {
        public int Id { get; set; }
        public int RequiredProductId { get; set; }
        public int TargetProductId { get; set; }
        public int RequiredPerOneDiscounted { get; set; }
        public decimal TargetProductDiscountedBy { get; set; }
    }
}
