using System.Collections.Generic;

namespace WebShop.Models
{
    public class BasketWithPrice
    {
        public int BasketId { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public IEnumerable<DiscountedBasketItem> DiscountedItems { get; set; }
    }
}
