﻿using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationKernel;
using FluentAssertions;
using Moq;
using Utilities;
using WebShop.BasketItems;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Infrastructure;

namespace WebShop.Tests.GetBasketWithDiscountsAppliedTest
{
    internal class Fixture
    {
        private readonly WebShopDbContext _db = TestServiceFactory.Database();
        private readonly DataContainer _data = new DataContainer();
        private readonly List<Discount> _discounts = new List<Discount>();
        private readonly List<BasketItem> _basketItems = new List<BasketItem>();
        private decimal _expectedBasketPrice;

        internal Fixture WithDiscounts(params Discount[] discounts)
        {
            _discounts.AddRange(discounts);
            return this;
        }

        internal Fixture WithBasketItemsOf(Product product, int count)
        {
            var basketItems = Collections.New(() => _data.BasketItem(product), count);
            _basketItems.AddRange(basketItems);
            return this;
        }

        internal Fixture WithBasketItemOf(Product product)
        {
            _basketItems.Add(_data.BasketItem(product));
            return this;
        }

        internal Fixture BasketPriceShouldBe(decimal expectedBasketPrice)
        {
            _expectedBasketPrice = expectedBasketPrice;
            return this;
        }

        internal async Task Run()
        {
            var basket = _data.Basket;
            _db.AddRange(basket);
            _db.AddRange(_discounts);
            _db.AddRange(_basketItems);
            _db.SaveChanges();

            var basketItemService = new BasketItemService(1);

            var sut = new BasketService(
                _db.Baskets,
                _db.Discounts,
                Mock.Of<IEventDispatcher>(),
                DiscountService.ShouldApplyToBasketItem,
                basketItemService.CalculateItemPrice
            );

            var returnedBasket = await sut.GetBasketWithDiscountsApplied(basket.Id);
            returnedBasket.Should().NotBeNull();
            returnedBasket.Id.Should().Be(returnedBasket.Id);
            returnedBasket.TotalPrice.Should().Be(_expectedBasketPrice);
        }
    }
}