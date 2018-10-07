using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using WebShop.Baskets;
using WebShop.Discounts;
using Xunit;

namespace WebShop.Tests
{
    public class BasketItemInitializerShould
    {
        [Fact]
        public async Task Set_price_equal_to_regular_price()
        {
            var product = new Product
            {
                Id = 1,
                RegularPrice = 50
            };

            var basket = new Basket
            {
                Id = 1,
                GrantedDiscounts = new GrantedDiscount[0]
            };

            var item = new BasketItem
            {
                Id = 1,
                ProductId = product.Id,
                Product = product,
                BasketId = basket.Id,
                Basket = basket,
            };

            var sut = new BasketItemService();
            await sut.Initialize(item);
            item.Price.Should().Be(product.RegularPrice);
        }

        [Fact]
        public async Task Set_discounted_price()
        {
            var product = new Product
            {
                Id = 1,
                RegularPrice = 50
            };

            var discounts = new[]
            {
                new Discount
                {
                    ProductId = 1,
                    MaxNumberOfItemsToApplyTo = 1,
                    RequiredMinimalQuantity = 1,
                    Value = (decimal) 0.1
                }
            };

            var basketItemId = 1;

            var basket = new Basket
            {
                Id = 1,
                GrantedDiscounts = new[]
                {
                    new GrantedDiscount
                    {
                        DiscountId = discounts[0].Id,
                        Discount = discounts[0],
                        ItemId = basketItemId
                    }
                }
            };

            var item = new BasketItem
            {
                Id = basketItemId,
                ProductId = product.Id,
                Product = product,
                BasketId = basket.Id,
                Basket = basket,
            };

            var sut = new BasketItemService();
            await sut.Initialize(item);
            item.Price.Should().Be(45);
        }
    }
}
