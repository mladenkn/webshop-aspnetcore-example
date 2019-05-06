using System;
using MediatR;
using WebShop.Models;

namespace WebShop
{
    public class BasketPriceRequested : INotification
    {
        public BasketPriceRequested(BasketWithPrice basket)
        {
            Basket = basket;
        }

        public BasketWithPrice Basket { get; }
        public DateTime DateTime { get; } = DateTime.Now;
    }
}
