using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using FluentAssertions.Collections;
using Utilities;
using WebShop.Baskets;
using WebShop.Discounts;
using WebShop.Queries;
using Xunit;

namespace WebShop.Tests
{
    public class GetBasketWithDiscountsAppliedTest
    {
        private readonly Fixture _fixture = new Fixture();
        private readonly DataGenerator _data = new DataGenerator();

        public GetBasketWithDiscountsAppliedTest()
        {
            _fixture.WithBasket(_data.Basket);
        }

        //[Fact]
        //public async Task Buy_1_milk____get_1_milk_discounted()
        //{
        //    var discount = new Discount
        //    {
        //        ForProductId = _data.Milk.Id,
        //        RequiredProductId = _data.Milk.Id,
        //        RequiredProductQuantity = 1,
        //        TargetProductQuantity = int.MaxValue
        //    };

        //    var basketItem = _data.BasketItem(_data.Milk);

        //    await _fixture
        //        .WithProducts(_data.Milk)
        //        .WithBasketItems(basketItem)
        //        .WithDiscounts(discount)
        //        .BasketItemsShould(_ =>
        //        {
        //            _.ContainSingle(bi => bi.Discounts.ContainsOne(d => d.Id == discount.Id) &&
        //                                  bi.ProductId == _data.Milk.Id);
        //        })
        //        .Run();
        //}

        //[Fact]
        //public async Task Buy_2_butters_and_2_milks___get_1_milk_discounted()
        //{
        //    var basketItems = Collections.Concat(
        //        Collections.New(() => _data.BasketItem(_data.Butter), 2),
        //        Collections.New(() => _data.BasketItem(_data.Milk), 2)
        //    );
            
        //    var discount = new Discount
        //    {
        //        ForProductId = _data.Milk.Id,
        //        RequiredProductId = _data.Butter.Id,
        //        RequiredProductQuantity = 2,
        //        TargetProductQuantity = 1
        //    };

        //    await _fixture
        //        .WithProducts(_data.Butter, _data.Milk)
        //        .WithBasketItems(basketItems)
        //        .WithDiscounts(discount)
        //        .BasketItemsShould(_ =>
        //        {
        //            _.ContainSingle(bi => bi.Discounts.ContainsOne(d => d.Id == discount.Id) &&
        //                                  bi.ProductId == _data.Milk.Id);
        //        })
        //        .Run();
        //}

        [Fact]
        public async Task Basket_has___1_bread_1_butter_1__milk()
        {
            var basketItems = new[]
            {
                _data.BasketItem(_data.Bread),
                _data.BasketItem(_data.Butter),
                _data.BasketItem(_data.Milk)
            };

            await _fixture
                .WithProducts(_data.Bread, _data.Bread, _data.Milk)
                .WithBasketItems(basketItems)
                .BasketPriceShouldBe(2.95m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butters_2_breads()
        {
            var basketItems = new[]
            {
                _data.BasketItem(_data.Bread),
                _data.BasketItem(_data.Bread),
                _data.BasketItem(_data.Butter),
                _data.BasketItem(_data.Butter),
            };

            await _fixture
                .WithProducts(_data.Bread, _data.Butter)
                .WithDiscounts(_data.BreadDiscount)
                .WithBasketItems(basketItems)
                .BasketPriceShouldBe(3.1m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___4_milks()
        {
            var basketItems = new[]
            {
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
            };

            await _fixture
                .WithProducts(_data.Milk)
                .WithDiscounts(_data.MilkDiscount)
                .WithBasketItems(basketItems)
                .BasketPriceShouldBe(3.45m)
                .Run();
        }

        [Fact]
        public async Task Basket_has___2_butter_1_bread_8_milk()
        {
            var basketItems = new[]
            {
                _data.BasketItem(_data.Butter),
                _data.BasketItem(_data.Butter),
                _data.BasketItem(_data.Bread),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
                _data.BasketItem(_data.Milk),
            };

            await _fixture
                .WithProducts(_data.Milk, _data.Bread, _data.Butter)
                .WithDiscounts(_data.MilkDiscount, _data.BreadDiscount)
                .WithBasketItems(basketItems)
                .BasketPriceShouldBe(9)
                .Run();
        }

        private class Fixture
        {
            private readonly WebShopDbContext _db = TestServiceFactory.Database();

            private readonly List<Product> _products = new List<Product>();
            private readonly List<Discount> _discounts = new List<Discount>();
            private readonly List<BasketItem> _basketItems = new List<BasketItem>();
            private Basket _basket;
            private decimal _expectedBasketPrice;

            public Fixture WithBasket(Basket basket)
            {
                _basket = basket;
                return this;
            }

            public Fixture WithProducts(params Product[] products)
            {
                _products.AddRange(products);
                return this;
            }

            public Fixture WithDiscounts(params Discount[] discounts)
            {
                _discounts.AddRange(discounts);
                return this;
            }

            public Fixture WithBasketItems(params BasketItem[] basketItems)
            {
                _basketItems.AddRange(basketItems);
                return this;
            }

            public Fixture BasketPriceShouldBe(decimal expectedBasketPrice)
            {
                _expectedBasketPrice = expectedBasketPrice;
                return this;
            }

            public async Task Run()
            {
                _db.AddRange(_basket);
                _db.AddRange(_products);
                _db.AddRange(_discounts);
                _db.AddRange(_basketItems);
                _db.SaveChanges();

                var sut = new BasketQueries(_db.Baskets, _db.Discounts, 1);
                var basket = await sut.GetBasketWithDiscountsApplied(_basket.Id);
                basket.Should().NotBeNull();
                basket.Id.Should().Be(_basket.Id);
                basket.TotalPrice.Should().Be(_expectedBasketPrice);
            }
        }

        private class DataGenerator
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

            public DataGenerator()
            {
                MilkDiscount = new Discount
                {
                    ForProductId = Milk.Id,
                    RequiredProductId = Milk.Id,
                    RequiredProductQuantity = 3,
                    TargetProductQuantity = 1,
                    Value = 1
                };
                BreadDiscount = new Discount
                {
                    ForProductId = Bread.Id,
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
