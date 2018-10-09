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
    public class GrantDiscountsShould : BaseTest
    {
        [Fact]
        public async Task Grant_correct_discount()
        {
            var product = FakerOf<Product>().Generate();

            var discounts = FakerOfDiscount()
                .RuleForProduct(product)
                .Generate(1);

            var basketItems = FakerOf<BasketItem>()
                .RuleForProduct(product)
                .Generate(3);

            var basket = FakerOf<Basket>()
                .RuleFor(b => b.Items, basketItems)
                .Generate();

            var grantedDiscounts = await Act(discounts, basket);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Id == discounts[0].Id);
        }
        
        [Fact]
        public async Task Not_grant_discounts_when_there_isnt_minimal_quantity_of_items_in_basket()
        {
            var product = FakerOf<Product>().Generate();

            var discounts = FakerOfDiscount()
                .RuleForProduct(product)
                .Generate(1);

            var basketItems = FakerOf<BasketItem>()
                .RuleForProduct(product)
                .Generate(1);

            var basket = FakerOf<Basket>()
                .RuleFor(b => b.Items, basketItems)
                .Generate();

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

        private static Faker<Discount> FakerOfDiscount()
        {
            return new Faker<Discount>()
                .RuleFor(d => d.RequiredMinimalQuantity, 3)
                .RuleFor(d => d.MaxNumberOfItemsToApplyTo, 1)
                ;
        }
    }
}
