﻿using FluentAssertions;
using MediatR;
using Moq;
using WebShop.Baskets;
using WebShop.Discounts;
using Xunit;

namespace WebShop.Tests
{
    public class BasketServiceShould
    {
        private readonly BasketService _sut = new BasketService(Mock.Of<IMediator>());

        [Fact]
        public void Grant_discounts()
        {
            var product = new Product {Id = 1};

            var discounts = new[]
            {
                new Discount
                {
                    Name = "1",
                    ProductId = product.Id,
                    RequiredQuantity = 3
                }
            };

            var basket = new Basket
            {
                Items = new[]
                {
                    new Basket.Item
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = 3
                    }
                }
            };

            var grantedDiscounts = _sut.GrantDiscounts(basket, discounts);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().ContainSingle(d => d.Discount.Name == discounts[0].Name);
        }

        [Fact]
        public void Not_grant_discounts()
        {
            var product = new Product { Id = 1 };

            var discounts = new[]
            {
                new Discount
                {
                    Name = "1",
                    ProductId = product.Id,
                    RequiredQuantity = 3
                }
            };

            var basket = new Basket
            {
                Items = new[]
                {
                    new Basket.Item
                    {
                        ProductId = product.Id,
                        Product = product,
                        Quantity = 2
                    }
                }
            };

            var grantedDiscounts = _sut.GrantDiscounts(basket, discounts);

            grantedDiscounts.Should().NotBeNull();
            grantedDiscounts.Should().BeEmpty();
        }
    }
}