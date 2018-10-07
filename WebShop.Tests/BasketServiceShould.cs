using System;
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
using Xunit;

namespace WebShop.Tests
{
    public class BasketServiceShould
    {
        [Fact]
        public async Task Grant_correct_discount()
        {
            var product = new Product {Id = 1};

            var discounts = new Faker<Discount>()
                .RuleFor(d => d.ProductId, product.Id)
                .RuleFor(d => d.Product, product)
                .RuleFor(d => d.RequiredMinimalQuantity, 3)
                .RuleFor(d => d.MaxNumberOfItemsToApplyTo, 1)
                .Generate(1);

            var basket = new Faker<Basket>()
                .RuleFor(b => b.Items, f =>
                {
                    return new Faker<BasketItem>()
                        .RuleFor(i => i.ProductId, product.Id)
                        .RuleFor(i => i.Product, product)
                        .Generate(3);
                })
                .Generate();

            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable(), null);

            var grantedDiscounts = await sut.GrantDiscounts(basket);
            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Id == discounts[0].Id);
        }
        
        [Fact]
        public async Task Not_grant_discounts_when_there_isnt_minimal_quantity_of_items_in_basket()
        {
            var product = new Product { Id = 1};

            var discounts = new Faker<Discount>()
                .RuleFor(d => d.ProductId, product.Id)
                .RuleFor(d => d.Product, product)
                .RuleFor(d => d.RequiredMinimalQuantity, 3)
                .RuleFor(d => d.MaxNumberOfItemsToApplyTo, 1)
                .Generate(1);

            var basket = new Faker<Basket>()
                .RuleFor(b => b.Items, f =>
                {
                    return new Faker<BasketItem>()
                        .RuleFor(i => i.ProductId, product.Id)
                        .RuleFor(d => d.Product, product)
                        .Generate(1);
                })
                .Generate();

            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable(), null);

            var grantedDiscounts = await sut.GrantDiscounts(basket);
            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().BeEmpty();
        }
    }
}
