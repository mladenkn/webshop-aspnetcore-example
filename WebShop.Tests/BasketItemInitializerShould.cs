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
            var basket = Basket(new GrantedDiscount[0]).Generate();
            var product = Product().Generate();
            var basketItem = BasketItem(1, product, basket).Generate();

            await Act(basketItem);

            basketItem.Price.Should().Be(product.RegularPrice);
        }

        [Fact]
        public async Task Set_discounted_price()
        {
            var basketItemId = 1;
            var numberOfGrantedDiscounts = 1;

            var product = Product().Generate();

            var discount = new Faker<Discount>()
                .RuleFor(p => p.ProductId, product.Id)
                .RuleFor(p => p.MaxNumberOfItemsToApplyTo, 1)
                .RuleFor(p => p.RequiredMinimalQuantity, 1)
                .RuleFor(p => p.Value, (decimal) 0.1)
                .Generate();

            var grantedDiscounts = new Faker<GrantedDiscount>()
                .RuleFor(gd => gd.DiscountId, discount.Id)
                .RuleFor(gd => gd.Discount, discount)
                .RuleFor(gd => gd.ItemId, basketItemId)
                .Generate(numberOfGrantedDiscounts);

            var basket = Basket(grantedDiscounts).Generate();
            var item = BasketItem(basketItemId, product, basket).Generate();

            await Act(item);

            item.Price.Should().Be(45);
        }

        private static async Task Act(BasketItem basketItem)
        {
            var sut = new BasketItemService();
            await sut.Initialize(basketItem);
        }

        private Faker<Product> Product()
        {
            return new Faker<Product>()
                .RuleFor(p => p.Id, 1)
                .RuleFor(p => p.RegularPrice, 50)
                ;
        }

        private Faker<BasketItem> BasketItem(int id, Product product, Basket basket)
        {
            return new Faker<BasketItem>()
                .RuleFor(i => i.Id, id)
                .RuleFor(i => i.BasketId, f => basket.Id)
                .RuleFor(i => i.Basket, f => basket)
                .RuleFor(i => i.ProductId, product.Id)
                .RuleFor(i => i.Product, product)
                ;
        }

        private Faker<Basket> Basket(IReadOnlyCollection<GrantedDiscount> grantedDiscounts)
        {
            return new Faker<Basket>()
                .RuleFor(b => b.Id, 1)
                .RuleFor(b => b.GrantedDiscounts, grantedDiscounts)
                ;
        }
    }
}
