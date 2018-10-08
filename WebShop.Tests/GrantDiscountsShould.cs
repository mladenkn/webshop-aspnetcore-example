using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bogus;
using FluentAssertions;
using FluentAssertions.Collections;
using MediatR;
using Moq;
using WebShop.Abstract;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Tests.Abstract;
using Xunit;

namespace WebShop.Tests
{
    public class GrantDiscountsShould : FakerContainerAware
    {
        private void ArrangeDataGenerators(int numberOfBasketItems)
        {
            FakerOf<Product>()
                .RuleFor(p => p.Id, 1)
                ;

            var product = FakerOf<Product>().Generate();

            FakerOf<Discount>()
                .RuleFor(d => d.ProductId, product.Id)
                .RuleFor(d => d.RequiredMinimalQuantity, 3)
                .RuleFor(d => d.MaxNumberOfItemsToApplyTo, 1)
                ;

            FakerOf<BasketItem>()
                .RuleFor(i => i.ProductId, product.Id)
                .RuleFor(i => i.Product, product)
                ;

            FakerOf<Basket>()
                .RuleFor(b => b.Items, f => FakerOf<BasketItem>().Generate(numberOfBasketItems))
                ;
        }

        private async Task<IEnumerable<GrantedDiscount>> Act(IEnumerable<Discount> discounts)
        {
            var basket = FakerOf<Basket>().Generate();
            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable(), null);
            var grantedDiscounts = await sut.GrantDiscounts(basket);
            return grantedDiscounts;
        }

        [Fact]
        public async Task Grant_correct_discount()
        {
            ArrangeDataGenerators(numberOfBasketItems: 3);
            var discounts = FakerOf<Discount>().Generate(1);
            var grantedDiscounts = await Act(discounts);
            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Id == discounts[0].Id);
        }
        
        [Fact]
        public async Task Not_grant_discounts_when_there_isnt_minimal_quantity_of_items_in_basket()
        {
            ArrangeDataGenerators(numberOfBasketItems: 1);
            var discounts = FakerOf<Discount>().Generate(1);
            var grantedDiscounts = await Act(discounts);
            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().BeEmpty();
        }
    }
}
