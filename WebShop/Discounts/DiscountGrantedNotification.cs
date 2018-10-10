using MediatR;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public class DiscountGrantedNotification : INotification
    {
        public DiscountGrantedNotification(BasketItem basketItem)
        {
            BasketItem = basketItem;
        }

        public BasketItem BasketItem { get;}
    }
}
