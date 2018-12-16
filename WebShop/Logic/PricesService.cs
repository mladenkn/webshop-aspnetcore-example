using System.Linq;
using MediatR;
using Utilities;
using WebShop.Models;

namespace WebShop.Logic
{
    public interface IPricesService
    {
        void RefreshPriceOf(BasketItem item);
        void RefreshPriceOf(Basket basket);
    }

    public class PricesService : IPricesService
    {
        private readonly decimal _maxAllowedDiscount;
        private readonly IMediator _mediator;

        public PricesService(decimal maxAllowedDiscount, IMediator mediator)
        {
            _maxAllowedDiscount = maxAllowedDiscount;
            _mediator = mediator;
        }

        public void RefreshPriceOf(Basket basket)
        {
            basket.Items.Must().NotBeNull();
            basket.Price = basket.Items.Sum(i => i.Price);
            _mediator.Publish(new BasketPriceRequested(basket));
        }

        public void RefreshPriceOf(BasketItem item)
        {
            item.Discounts.Must().NotBeNull();
            item.Product.Must().NotBeNull();

            var totalDiscount = item.Discounts.Select(d => d.Discount.Value).Sum();
            if (totalDiscount > _maxAllowedDiscount)
                totalDiscount = _maxAllowedDiscount;

            var without = item.Product.RegularPrice * totalDiscount;
            item.Price = item.Product.RegularPrice - without;
        }
    }
}
