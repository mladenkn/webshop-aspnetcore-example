using System.Collections.Generic;

namespace WebShop.Models
{
    public class BasketWithPrice
    {
        public class Item
        {
            public int BasketItemId { get; set; }
            public int ProductId { get; set; }
            public int? DiscountId { get; set; }
            public decimal Price { get; set; }
        }

        public int BasketId { get; set; }
        public decimal Price { get; set; }
        public IEnumerable<Item> Items { get; set; }
    }
}
