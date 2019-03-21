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

namespace WebShop.Tests
{
    public class ApplyDiscountsTest
    {
        //[Theory]
        //[InlineData(0, 0, 1, 0)]
        //[InlineData(0, 0, 2, 0)]
        //[InlineData(0, 1, 1, 0)]
        //[InlineData(0, 1, 2, 0)]
        //[InlineData(1, 0, 1, 0)]
        //[InlineData(1, 0, 2, 0)]
        //[InlineData(1, 1, 1, 1)]
        //[InlineData(1, 1, 2, 0)]
        //[InlineData(1, 2, 2, 0)]
        //[InlineData(2, 1, 1, 1)]
        //[InlineData(2, 1, 2, 1)]
        //[InlineData(2, 1, 3, 0)]
        //[InlineData(2, 2, 1, 1)]
        //[InlineData(2, 2, 2, 1)]
        //[InlineData(2, 2, 2, 2)]
        //[InlineData(2, 2, 3, 0)]
        //[InlineData(3, 4, 3, 1)]
        //public async Task Run(
        //    int numberOfProducts1Purchased,
        //    int numberOfProducts2Purchased,
        //    int requiredQuantityOfProduct1ToReceiveDiscount,
        //    int numberOfProducts2ThatShouldReceiveDiscount)
        //{
        //    var db = TestServiceFactory.InMemoryDatabase();
        //    var smartQueries = new SmartQueries(db, null);
        //    var discountService = new DiscountService(smartQueries);

        //    var product1 = new Product { Id = 1 };
        //    var product2 = new Product { Id = 2 };

        //    var basket = new Basket { Items = new List<BasketItem>() };

        //    for (var i = 0; i < numberOfProducts1Purchased; i++)
        //        basket.Items.Add(new BasketItem { ProductId = product1.Id });

        //    for (var i = 0; i < numberOfProducts2Purchased; i++)
        //        basket.Items.Add(new BasketItem { ProductId = product2.Id });

            //var discount = new Discount
            //{
            //    Id = 1,
            //    RequiredProductId = product1.Id,
            //    TargetProductId = product2.Id,
            //    RequiredProductRequiredQuantity = requiredQuantityOfProduct1ToReceiveDiscount,
            //    TargetProductQuantity = numberOfProducts2ThatShouldReceiveDiscount
            //};

            //await db.PersistAll(basket, product1, product2, basket.Items, discount);

            //var appliedDiscounts = await discountService.ApplyDiscounts(basket);

            //appliedDiscounts.Should().HaveCount(numberOfProducts2ThatShouldReceiveDiscount);
            //appliedDiscounts.All(ap => ap.BasketItem.ProductId == product2.Id &&
            //                           ap.DiscountId == discount.Id)
            //    .Should().BeTrue();
        //}

        [Fact]
        public async Task Run()
        {
            var butter = new Product { Id = 1 };
            var milk = new Product { Id = 2 };
            var bread = new Product { Id = 3 };

            var twoButtersDiscount = Discount.New()
                .Require(productId: butter.Id, requiredQuantity: 2)
                .DiscountFor(productId: bread.Id, quantity: 1, value: 0.5m)
                .Build();

            var threeMilksDiscount = Discount.New()
                .Require(productId: milk.Id, requiredQuantity: 3)
                .DiscountFor(productId: milk.Id, quantity: 1, value: 0.5m)
                .Build();

            ArgsBuilder BaseArgs() => new ArgsBuilder()
                .Products(butter, milk, bread)
                .Discounts(twoButtersDiscount, threeMilksDiscount);

            await Run_(BaseArgs()
                .BasketItemsOfProduct(butter, 1)
                .BasketItemsOfProduct(milk, 1)
                .BasketItemsOfProduct(bread, 1)
            );

            await Run_(BaseArgs()
                .BasketItemsOfProduct(butter, 2)
                .BasketItemsOfProduct(bread, 2)
                .ShouldApplyDiscount(twoButtersDiscount)
            );

            await Run_(BaseArgs()
                .BasketItemsOfProduct(milk, 4)
                .ShouldApplyDiscount(threeMilksDiscount)
            );

            await Run_(BaseArgs()
                .BasketItemsOfProduct(milk, 8)
                .BasketItemsOfProduct(butter, 2)
                .BasketItemsOfProduct(bread, 1)
                .ShouldApplyDiscount(threeMilksDiscount)
                .ShouldApplyDiscount(twoButtersDiscount)
            );
        }

        public async Task Run_(ArgsBuilder argsBuilder)
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var smartQueries = new SmartQueries(db);
            var sut = new BasketService(smartQueries, Mock.Of<IMediator>());

            var a = argsBuilder.Args;

            var basket = new Basket {Items = a.BasketItems};
            a.BasketItems.ForEach(bi => bi.BasketId = basket.Id);
            
            var allDiscountsRequiredProducts = a.Discounts.Select(d => d.RequiredProducts).SelectMany(o => o);
            var allDiscountsMicroDiscounts = a.Discounts.Select(d => d.MicroDiscounts).SelectMany(o => o);
            var allModels = General.ConcatAll(a.Products, a.Discounts, allDiscountsRequiredProducts, allDiscountsMicroDiscounts).Append(basket);
            allModels.ForEach(o => db.Add(o));
            await db.SaveChangesAsync();

            await sut.ApplyDiscounts(basket);

            foreach (var aAppliedDiscountsId in a.AppliedDiscountsIds)
                basket.AppliedDiscounts.Should().Contain(ad => ad.DiscountId == aAppliedDiscountsId);
        }
    }

    public class Args
    {
        public List<Product> Products { get; } = new List<Product>();
        public List<BasketItem> BasketItems { get; } = new List<BasketItem>();
        public List<Discount> Discounts { get; } = new List<Discount>();
        public List<Guid> AppliedDiscountsIds { get; } = new List<Guid>();
        public decimal ExpectedBasketPrice { get; set; }
    }

    public class ArgsBuilder
    {
        public Args Args { get; } = new Args();

        public ArgsBuilder Products(params Product[] products)
        {
            Args.Products.AddRange(products);
            return this;
        }

        public ArgsBuilder BasketItemsOfProduct(Product product, int times)
        {
            for (var i = 0; i < times; i++)
                Args.BasketItems.Add(new BasketItem {ProductId = product.Id});
            return this;
        }

        public ArgsBuilder Discounts(params Discount[] discounts)
        {
            Args.Discounts.AddRange(discounts);
            return this;
        }

        public ArgsBuilder ShouldApplyDiscount(Discount discount)
        {
            Args.AppliedDiscountsIds.Add(discount.Id);
            return this;
        }

        public ArgsBuilder ExpectedBasketPrice(decimal price)
        {
            Args.ExpectedBasketPrice = price;
            return this;
        }
    }
}
