using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        public PricesService(decimal maxAllowedDiscount)
        {
            _maxAllowedDiscount = maxAllowedDiscount;
        }

        public void RefreshPriceOf(Basket basket)
        {
            basket.Items.Must().NotBeNull();
            basket.Price = basket.Items.Sum(i => i.Price);
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
