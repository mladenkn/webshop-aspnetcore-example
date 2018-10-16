using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Utilities;
using WebShop.Abstract;
using WebShop.Baskets;

namespace WebShop.Discounts
{
    public delegate Task ApplyDiscountToBasketItem(BasketItem basketItem, Discount discount, IDatabaseTransaction databaseTransaction);

    public class DiscountService
    {
        private readonly IMediator _mediator;

        public DiscountService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public Task ApplyDiscountToBasketItem(BasketItem basketItem, Discount discount, IDatabaseTransaction transaction)
        {
            basketItem.IsDiscounted = true;
            var basketDiscount = new BasketItemDiscount { BasketItemId = basketItem.Id, DiscountId = discount.Id };
            return new[]
            {
                new DiscountGrantedNotification(basketItem).PublishWith(_mediator),
                transaction.Update(basketItem).Save(basketDiscount).Commit()
            }
            .WhenAll();
        }
    }
}
