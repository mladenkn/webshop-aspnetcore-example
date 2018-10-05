using System;
using System.Collections.Generic;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public struct GrantedDiscount
    {
        public GrantedDiscount(Discount discount, IReadOnlyCollection<BasketItem> items) : this()
        {
            Discount = discount;
            Items = items;
        }

        public Discount Discount { get; }

        public IReadOnlyCollection<BasketItem> Items { get; }
    }
}
