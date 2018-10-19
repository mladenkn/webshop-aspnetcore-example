using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using Utilities;
using WebShop.Abstract;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Infrastructure;
using Xunit;

namespace WebShop.Tests
{
    public class GetBasketWithDiscountsAppliedTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly DataContainer _data = new DataContainer();

        [Fact]
        public async Task Basket_has___1_bread_1_butter_1__milk()
        {
            await _fixture
                .WithBasketItemOf(_data.Butter)
                .WithBasketItemOf(_data.Bread)
                .WithBasketItemOf(_data.Milk)
                .BasketPriceShouldBe(2.95m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butters_2_breads()
        {
            await _fixture
                .WithDiscounts(_data.BreadDiscount)
                .WithBasketItemsOf(_data.Butter, 2)
                .WithBasketItemsOf(_data.Bread, 2)
                .BasketPriceShouldBe(3.1m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___4_milks()
        {
            await _fixture
                .WithDiscounts(_data.MilkDiscount)
                .WithBasketItemsOf(_data.Milk, 4)
                .BasketPriceShouldBe(3.45m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butter_1_bread_8_milk()
        {
            await _fixture
                .WithDiscounts(_data.MilkDiscount, _data.BreadDiscount)
                .WithBasketItemsOf(_data.Butter, 2)
                .WithBasketItemOf(_data.Bread)
                .WithBasketItemsOf(_data.Milk, 8)
                .BasketPriceShouldBe(9)
                .Run();
        }

        private class Fixture
        {
            private readonly WebShopDbContext _db = TestServiceFactory.Database();
            private readonly DataContainer _data = new DataContainer();
            private readonly List<Discount> _discounts = new List<Discount>();
            private readonly List<BasketItem> _basketItems = new List<BasketItem>();
            private decimal _expectedBasketPrice;

            public Fixture WithDiscounts(params Discount[] discounts)
            {
                _discounts.AddRange(discounts);
                return this;
            }

            public Fixture WithBasketItemsOf(Product product, int count)
            {
                var basketItems = Collections.New(() => _data.BasketItem(product), count);
                _basketItems.AddRange(basketItems);
                return this;
            }

            public Fixture WithBasketItemOf(Product product)
            {
                _basketItems.Add(_data.BasketItem(product));
                return this;
            }

            public Fixture BasketPriceShouldBe(decimal expectedBasketPrice)
            {
                _expectedBasketPrice = expectedBasketPrice;
                return this;
            }

            public async Task Run()
            {
                var basket = _data.Basket;
                _db.AddRange(basket);
                _db.AddRange(_discounts);
                _db.AddRange(_basketItems);
                _db.SaveChanges();

                var sut = new BasketQueries(_db.Baskets, _db.Discounts, 1, Mock.Of<IEventDispatcher>());
                var returnedBasket = await sut.GetBasketWithDiscountsApplied(basket.Id);
                returnedBasket.Should().NotBeNull();
                returnedBasket.Id.Should().Be(returnedBasket.Id);
                returnedBasket.TotalPrice.Should().Be(_expectedBasketPrice);
            }
        }

        private class DataContainer
        {
            public Basket Basket { get; } = new Basket { Id = 1 };

            public Product Butter { get; } = new Product
            {
                Id = 1,
                RegularPrice = 0.8m
            };

            public Product Milk { get; } = new Product
            {
                Id = 2,
                RegularPrice = 1.15m
            };

            public Product Bread { get; } = new Product
            {
                Id = 3,
                RegularPrice = 1.0m
            };

            public Discount BreadDiscount { get; }
            public Discount MilkDiscount { get; }

            public DataContainer()
            {
                MilkDiscount = new Discount
                {
                    TargetProductId = Milk.Id,
                    RequiredProductId = Milk.Id,
                    RequiredProductQuantity = 3,
                    TargetProductQuantity = 1,
                    Value = 1
                };
                BreadDiscount = new Discount
                {
                    TargetProductId = Bread.Id,
                    RequiredProductId = Butter.Id,
                    RequiredProductQuantity = 2,
                    TargetProductQuantity = 1,
                    Value = 0.5m
                };
            }

            public BasketItem BasketItem(Product product)
            {
                return new BasketItem
                {
                    Basket = Basket,
                    BasketId = Basket.Id,
                    Product = product,
                    ProductId = product.Id
                };
            }
        }
    }
}
