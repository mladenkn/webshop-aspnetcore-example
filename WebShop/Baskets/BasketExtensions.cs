using System;
using System.Collections.Generic;
using System.Linq;

namespace WebShop.Baskets
{
    public static class BasketExtensions
    {
        public static Basket ToSingleBasket(this IEnumerable<BasketItem> items)
        {
            if(items.Select(i => i.ProductId).AnyDuplicates())
                throw new ApplicationException("Basket Items of multiple users found");

            var basket = new Basket();

            basket.UserId = items.First().UserId;
            basket.User = items.First().User;
            basket.Items = items.ToList();

            return basket;
        }
    }
}
