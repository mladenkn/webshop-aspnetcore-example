using System;
using System.Collections.Generic;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public struct GrantedDiscount
    {
        public GrantedDiscount(Discount discount, IReadOnlyCollection<Basket.Item> items) : this()
        {
            Discount = discount;
            Items = items;
        }

        public Discount Discount { get; }

        public IReadOnlyCollection<Basket.Item> Items { get; }
    }
}
