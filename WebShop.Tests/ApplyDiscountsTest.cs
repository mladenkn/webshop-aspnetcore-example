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

            var twoButtersDiscount = new BasketDiscount
            {
                Id = 1,
                RequiredProductId = butter.Id,
                TargetProductId = bread.Id,
                RequiredPerOneDiscounted = 2,
                TargetProductDiscountedBy = 0.5m
            };

            var threeMilksDiscount = new BasketDiscount
            {
                Id = 2,
                RequiredProductId = milk.Id,
                TargetProductId = milk.Id,
                RequiredPerOneDiscounted = 3,
                TargetProductDiscountedBy = 1
            };

            var otherDiscounts = new[]
            {
                new BasketDiscount
                {
                    Id = 3,
                    RequiredProductId = butter.Id,
                    TargetProductId = butter.Id,
                    RequiredPerOneDiscounted = 5,
                    TargetProductDiscountedBy = 2
                },
                new BasketDiscount
                {
                    Id = 4,
                    RequiredProductId = Product(6).Id,
                    TargetProductId = butter.Id,
                    RequiredPerOneDiscounted = 5,
                    TargetProductDiscountedBy = 2
                },
            };

            void AddBaseArgs(Args a)
            {
                a.Products = new[] { butter, milk, bread, Product(4), Product(5)};
                a.Discounts = new[] {twoButtersDiscount, threeMilksDiscount}.Concat(otherDiscounts);
            }

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = new BasketItem[0];
                a.AppliedDiscounts = new Args.AppliedDiscount[0];
            });

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = new[] { BasketItem(bread), BasketItem(butter), BasketItem(milk) };
                a.AppliedDiscounts = new Args.AppliedDiscount[0];
            });

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = new[] { BasketItem(butter), BasketItem(butter), BasketItem(bread), BasketItem(bread) };
                a.AppliedDiscounts = new[]
                {
                    AppliedDiscount(bread, twoButtersDiscount, 1),
                };
            });

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = GenerateSequence(() => BasketItem(milk), 4);
                a.AppliedDiscounts = new[]
                {
                    AppliedDiscount(milk, threeMilksDiscount, 1)
                };
            });

            await RunWithArgs(a =>
            {
                AddBaseArgs(a);
                a.BasketItems = GenerateSequence(() => BasketItem(milk), 8)
                    .Append(BasketItem(butter))
                    .Append(BasketItem(butter))
                    .Append(BasketItem(bread))
                ;
                a.AppliedDiscounts = new[]
                {
                    AppliedDiscount(bread, twoButtersDiscount, 1),
                    AppliedDiscount(milk, threeMilksDiscount, 2)
                };
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
            
            var allModels = ConcatAll(a.Products, a.Discounts).Append(basket);
            allModels.ForEach(o => db.Add(o));
            await db.SaveChangesAsync();

            var basketWithPrice = await sut.ApplyDiscounts(basket);

            basketWithPrice.DiscountedItems.Count().Should()
                .Be(a.AppliedDiscounts.Sum(ad => ad.NumberOfBasketItemsThatShouldReceiveIt));

            foreach (var appliedDiscount in a.AppliedDiscounts)
            {
                var count = basketWithPrice.DiscountedItems.Count(i =>
                    i.BasketItem.ProductId == appliedDiscount.ProductId &&
                    i.DiscountId == appliedDiscount.DiscountId
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
            public int DiscountId { get; set; }
            public int NumberOfBasketItemsThatShouldReceiveIt { get; set; }
        }
    }
}
