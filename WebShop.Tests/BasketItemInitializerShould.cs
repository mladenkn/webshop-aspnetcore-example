using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
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
            var basket = FakerOf<Basket>()
                .RuleFor(b => b.GrantedDiscounts, new GrantedDiscount[0])
                .Generate();

            var product = FakerOfProduct().Generate();

            var basketItem = FakerOf<BasketItem>()
                .RuleForProduct(product)
                .RuleForBasket(basket)
                .Generate();

            await Act(basketItem);

            basketItem.Price.Should().Be(product.RegularPrice);
        }

        [Fact]
        public async Task Set_discounted_price()
        {
            var basketItemId = 1;
            var numberOfGrantedDiscounts = 1;

            var product = FakerOfProduct().Generate();

            var discount = FakerOf<Discount>()
                .RuleFor(p => p.ProductId, product.Id)
                .RuleFor(p => p.MaxNumberOfItemsToApplyTo, 1)
                .RuleFor(p => p.RequiredMinimalQuantity, 1)
                .RuleFor(p => p.Value, (decimal) 0.1)
                .Generate();

            var grantedDiscounts = FakerOf<GrantedDiscount>()
                .RuleFor(gd => gd.DiscountId, discount.Id)
                .RuleFor(gd => gd.Discount, discount)
                .RuleFor(gd => gd.ItemId, basketItemId)
                .Generate(numberOfGrantedDiscounts);

            var basket = FakerOf<Basket>()
                .RuleFor(b => b.GrantedDiscounts, grantedDiscounts)
                .Generate();

            var item = FakerOf<BasketItem>()
                .RuleFor(i => i.Id, basketItemId)
                .RuleForProduct(product)
                .RuleForBasket(basket)
                .Generate();

            await Act(item);

            item.Price.Should().Be(45);
        }

        private static async Task Act(BasketItem basketItem)
        {
            var sut = new BasketItemService();
            await sut.Initialize(basketItem);
        }

        private static Faker<Product> FakerOfProduct()
        {
            return new Faker<Product>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.RegularPrice, 50)
                ;
        }

        private static Faker<T> FakerOf<T>() where T : class => new Faker<T>();
    }
}
