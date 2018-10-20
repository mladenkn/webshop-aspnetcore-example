using System.Linq;
using Utilities;

namespace WebShop.BasketItems
{
    public delegate decimal CalculateBasketItemPrice(BasketItem basketItem);

    public class BasketItemService
    {
        private readonly decimal _maxAllowedDiscount;

        public BasketItemService(decimal maxAllowedDiscount)
        {
            _maxAllowedDiscount = maxAllowedDiscount;
        }

        public decimal CalculateItemPrice(BasketItem item)
        {
            item.Discounts.Must().NotBeNull();
            item.Product.Must().NotBeNull();

            var totalDiscount = item.Discounts.Select(d => d.Value).Sum();
            if (totalDiscount > _maxAllowedDiscount)
                totalDiscount = _maxAllowedDiscount;

            var without = item.Product.RegularPrice * totalDiscount;
            item.Price = item.Product.RegularPrice - without;

            return item.Price;
        }
    }
}
