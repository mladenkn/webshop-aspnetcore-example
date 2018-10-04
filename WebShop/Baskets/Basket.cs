using System.Collections.Generic;
using WebShop.Discounts;

namespace WebShop.Baskets
{
    public class Basket
    {
        public string UserId { get; set; }
        public IReadOnlyCollection<Item> Items { get; set; }

        public User User { get; set; }
        public IReadOnlyCollection<GrantedDiscount> GrantedDiscounts { get; set; }

        public struct Item
        {
            public long ProductId { get; set; }
            public int Quantity { get; set; }

            public Product Product { get; set; }
        }
    }
}
