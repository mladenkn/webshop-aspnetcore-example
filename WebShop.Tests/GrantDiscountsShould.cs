using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using MediatR;
using Moq;
using WebShop.Baskets;
using WebShop.Discounts;
using Xunit;

namespace WebShop.Tests
{
    public class GrantDiscountsShould
    {
        [Fact]
        public async Task Grant_correct_discount()
        {
            var product = Products().Generate();
            var discounts = Discounts(product).Generate(1);
            var basketItems = BasketItems(product).Generate(3);
            var basket = Baskets(product, basketItems).Generate();

            var grantedDiscounts = await Act(discounts, basket);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Id == discounts[0].Id);
        }
        
        [Fact]
        public async Task Not_grant_discounts_when_there_isnt_minimal_quantity_of_items_in_basket()
        {
            var product = Products().Generate();
            var discounts = Discounts(product).Generate(1);
            var basketItems = BasketItems(product).Generate(1);
            var basket = Baskets(product, basketItems).Generate();

            var grantedDiscounts = await Act(discounts, basket);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().BeEmpty();
        }

        private async Task<IEnumerable<GrantedDiscount>> Act(IEnumerable<Discount> discounts, Basket basket)
        {
            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable(), null);
            var grantedDiscounts = await sut.GrantDiscounts(basket);
            return grantedDiscounts;
        }

        private Faker<Product> Products()
        {
            return new Faker<Product>()
                    .RuleFor(p => p.Id, 1)
                ;
        }

        private Faker<Discount> Discounts(Product product)
        {
            return new Faker<Discount>()
                .RuleFor(d => d.ProductId, product.Id)
                .RuleFor(d => d.RequiredMinimalQuantity, 3)
                .RuleFor(d => d.MaxNumberOfItemsToApplyTo, 1)
                ;
        }

        private Faker<BasketItem> BasketItems(Product product)
        {
            return new Faker<BasketItem>()
                .RuleFor(i => i.ProductId, product.Id)
                .RuleFor(i => i.Product, product)
                ;
        }

        private Faker<Basket> Baskets(Product product, IReadOnlyCollection<BasketItem> basketItems)
        {
            return new Faker<Basket>()
                .RuleFor(b => b.Items, basketItems)
                ;
        }
    }
}
