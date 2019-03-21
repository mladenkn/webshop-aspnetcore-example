using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MediatR;
using Moq;
using Utilities;
using WebShop.DataAccess;
using WebShop.Infrastructure.DataAccess;
using WebShop.Logic;
using WebShop.Models;
using Xunit;
using static Utilities.General;

namespace WebShop.Tests
{
    public class ApplyDiscountsTest
    {
        Product Product(int id) => new Product {Id = id};

        BasketItem BasketItem(Product p) => new BasketItem {ProductId = p.Id};

        Args.AppliedDiscount AppliedDiscount(Product product, BasketDiscount basketDiscount, int times) =>
            new Args.AppliedDiscount
            {
                ProductId = product.Id,
                DiscountId = basketDiscount.Id,
                NumberOfBasketItemsThatShouldReceiveIt = times
            };

        [Fact]
        public async Task Run()
        {
            var butter = Product(1);
            var milk = Product(2);
            var bread = Product(3);

            var twoButtersDiscount = BasketDiscount.New()
                .Require(productId: butter.Id, requiredQuantity: 2)
                .DiscountFor(productId: bread.Id, quantity: 1, value: 0.5m)
                .Build();

            var threeMilksDiscount = BasketDiscount.New()
                .Require(productId: milk.Id, requiredQuantity: 3)
                .DiscountFor(productId: milk.Id, quantity: 1, value: 0.5m)
                .Build();

            void AddBaseArgs(Args a)
            {
                a.Products = new[] { butter, milk, bread};
                a.Discounts = new[] {twoButtersDiscount, threeMilksDiscount};
            }

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = GenerateSequence(() => BasketItem(milk), 8)
                    .Append(BasketItem(butter))
                    .Append(BasketItem(butter))
                    .Append(BasketItem(bread))
                ;
                a.AppliedDiscounts = new[] { AppliedDiscount(milk, threeMilksDiscount, 2) };
            });
        }

        private async Task RunWithArgs(Action<Args> buildArgs)
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var smartQueries = new SmartQueries(db);
            var sut = new BasketService(smartQueries, Mock.Of<IMediator>());

            var a = new Args();
            buildArgs(a);

            var basket = new Basket {Items = a.BasketItems.ToList()};
            a.BasketItems.ForEach(bi => bi.BasketId = basket.Id);
            
            var allDiscountsRequiredProducts = a.Discounts.Select(d => d.RequiredProducts).SelectMany(o => o);
            var allDiscountsMicroDiscounts = a.Discounts.Select(d => d.BasketItemDiscounts).SelectMany(o => o);
            var allModels = ConcatAll(a.Products, a.Discounts, allDiscountsRequiredProducts, allDiscountsMicroDiscounts).Append(basket);
            allModels.ForEach(o => db.Add(o));
            await db.SaveChangesAsync();

            await sut.ApplyDiscounts(basket);

            foreach (var appliedDiscount in a.AppliedDiscounts)
            {
                var count = basket.AppliedDiscounts.Count(ap =>
                    ap.BasketItem.ProductId == appliedDiscount.ProductId &&
                    ap.DiscountId == appliedDiscount.DiscountId
                );
                count.Should().Be(appliedDiscount.NumberOfBasketItemsThatShouldReceiveIt);
            }
        }
    }

    public class Args
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<BasketItem> BasketItems { get; set; }
        public IEnumerable<BasketDiscount> Discounts { get; set; }
        public IEnumerable<AppliedDiscount> AppliedDiscounts { get; set; }
        public decimal ExpectedBasketPrice { get; set; }

        public class AppliedDiscount
        {
            public int ProductId { get; set; }
            public Guid DiscountId { get; set; }
            public int NumberOfBasketItemsThatShouldReceiveIt { get; set; }
        }
    }
}
