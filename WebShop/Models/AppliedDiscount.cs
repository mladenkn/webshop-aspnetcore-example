using System;

namespace WebShop.Models
{
    public class AppliedDiscount
    {
        public int Id { get; set; }
        public int BasketItemId { get; set; }
        public Guid DiscountId { get; set; }
        public decimal Value { get; set; }
    }
}
