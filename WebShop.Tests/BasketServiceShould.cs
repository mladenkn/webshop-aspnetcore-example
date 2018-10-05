using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Moq;
using WebShop.Baskets;
using WebShop.Discounts;
using Xunit;

namespace WebShop.Tests
{
    public class BasketServiceShould
    {
        [Fact]
        public async Task Grant_discounts()
        {
            var product = new Product {Id = 1};

            var discounts = new[]
            {
                new Discount
                {
                    Id = 1,
                    ProductId = product.Id,
                    RequiredQuantity = 3
                }
            };

            var basket = new Basket
            {
                Items = new[]
                {
                    new BasketItem
                    {
                        ProductId = product.Id,
                        Product = product
                    },
                    new BasketItem
                    {
                        ProductId = product.Id,
                        Product = product
                    },
                    new BasketItem
                    {
                        ProductId = product.Id,
                        Product = product
                    },
                }
            };

            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable());
            var grantedDiscounts = await sut.GrantDiscounts(basket);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Id == discounts[0].Id);
        }

        [Fact]
        public async Task Not_grant_discounts()
        {
            var product = new Product { Id = 1 };

            var discounts = new[]
            {
                new Discount
                {
                    ProductId = product.Id,
                    RequiredQuantity = 3
                }
            };

            var basket = new Basket
            {
                Items = new[]
                {
                    new BasketItem
                    {
                        ProductId = product.Id,
                        Product = product
                    }
                }
            };

            var sut = new BasketService(Mock.Of<IMediator>(), discounts.AsQueryable());
            var grantedDiscounts = await sut.GrantDiscounts(basket);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().BeEmpty();
        }
    }
}
