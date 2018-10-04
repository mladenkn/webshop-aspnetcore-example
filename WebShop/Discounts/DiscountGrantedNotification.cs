using MediatR;

namespace WebShop.Discounts
{
    public class DiscountGrantedNotification : INotification
    {
        public DiscountGrantedNotification(GrantedDiscount discount)
        {
            Discount = discount;
        }

        public GrantedDiscount Discount { get;}
    }
}
