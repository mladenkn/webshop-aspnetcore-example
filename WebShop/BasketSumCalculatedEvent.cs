using WebShop.Models;

namespace WebShop
{
    public class BasketSumCalculatedEvent
    {
        public BasketSumCalculatedEvent(Basket basket)
        {
            Basket = basket;
        }

        public Basket Basket { get; }
    }
}
