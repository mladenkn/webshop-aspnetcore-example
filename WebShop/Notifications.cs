using MediatR;
using WebShop.Models;

namespace WebShop
{
    public class BasketPriceCalculated : INotification
    {
        public BasketPriceCalculated(BasketWithPrice basket)
        {
            Basket = basket;
        }

        public BasketWithPrice Basket { get; }
    }
}
