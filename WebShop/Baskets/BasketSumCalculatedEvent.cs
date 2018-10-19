namespace WebShop.Baskets
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
