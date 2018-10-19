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

        [Fact]
        public async Task Buy_one_product_and_get_it_discounted()
        {
            var product = _data.Product();

            var discount = new Discount
            {
                ForProductId = product.Id,
                RequiredProductId = product.Id,
                RequiredProductMinimalQuantity = 1,
                MaxNumberOfItemsToApplyTo = int.MaxValue
            };

            var basketItem = _data.BasketItem(product);

            await _fixture
                .WithProducts(product)
                .WithBasketItems(basketItem)
                .WithDiscounts(discount)
                .BasketItemsShould(_ =>
                {
                    _.ContainSingle(bi => bi.Discounts.ContainsOne(d => d.Id == discount.Id) &&
                                          bi.ProductId == product.Id);
                })
                .Run();
        }

        [Fact]
        public async Task Buy_one_product1_and_get_one_product2_discounted()
        {
            var product1 = _data.Product(1);
            var product2 = _data.Product(2);

            var basketItems = Collections.Concat(
                Collections.New(() => _data.BasketItem(product1), 2),
                Collections.New(() => _data.BasketItem(product2), 2)
            );
            
            var discount = new Discount
            {
                ForProductId = product2.Id,
                RequiredProductId = product1.Id,
                RequiredProductMinimalQuantity = 2,
                MaxNumberOfItemsToApplyTo = 1
            };

            await _fixture
                .WithProducts(product1, product2)
                .WithBasketItems(basketItems)
                .WithDiscounts(discount)
                .BasketItemsShould(_ =>
                {
                    _.ContainSingle(bi => bi.Discounts.ContainsOne(d => d.Id == discount.Id) &&
                                          bi.ProductId == product2.Id);
                })
                .Run();
        }

        private class Fixture
        {
            private readonly WebShopDbContext _db = TestServiceFactory.Database();

            private readonly List<Product> _products = new List<Product>();
            private readonly List<Discount> _discounts = new List<Discount>();
            private readonly List<BasketItem> _basketItems = new List<BasketItem>();
            private Action<GenericCollectionAssertions<BasketItem>> _assert;
            private Basket _basket;

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

            public Fixture WithBasketItems(IEnumerable<BasketItem> basketItems)
            { 
                _basketItems.AddRange(basketItems);
                return this;
            }

            public Fixture WithBasketItems(params BasketItem[] basketItems)
            {
                return WithBasketItems((IEnumerable<BasketItem>) basketItems);
            }

            public Fixture BasketItemsShould(Action<GenericCollectionAssertions<BasketItem>> assert)
            {
                _assert = assert;
                return this;
            }

            public async Task Run()
            {
                _db.AddRange(_basket);
                _db.AddRange(_products);
                _db.AddRange(_discounts);
                _db.AddRange(_basketItems);
                _db.SaveChanges();

                var sut = new BasketQueries(_db.Baskets, _db.Discounts, 100);
                var basket = await sut.GetBasketWithDiscountsApplied(_basket.Id);
                basket.Should().NotBeNull();
                basket.Id.Should().Be(_basket.Id);
                _assert(basket.Items.Should());
            }
        }

        private class DataGenerator
        {
            public Basket Basket { get; } = new Basket { Id = 1 };

            public Product Product(int id = 1) => new Product
            {
                Id = id,
                RegularPrice = 20
            };

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
