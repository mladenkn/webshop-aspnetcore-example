using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebShop.DataAccess;
using WebShop.Infrastructure.DataAccess;
using WebShop.Logic;
using WebShop.Models;
using Xunit;

namespace WebShop.Tests
{
    public class ApplyDiscountsTest
    {
        [Theory]
        [InlineData(0, 0, 1, 0)]
        [InlineData(0, 0, 2, 0)]
        [InlineData(0, 1, 1, 0)]
        [InlineData(0, 1, 2, 0)]
        [InlineData(1, 0, 1, 0)]
        [InlineData(1, 0, 2, 0)]
        [InlineData(1, 1, 1, 1)]
        [InlineData(1, 1, 2, 0)]
        [InlineData(1, 2, 2, 0)]
        [InlineData(2, 1, 1, 1)]
        [InlineData(2, 1, 2, 1)]
        [InlineData(2, 1, 3, 0)]
        [InlineData(2, 2, 1, 1)]
        [InlineData(2, 2, 2, 1)]
        [InlineData(2, 2, 2, 2)]
        [InlineData(2, 2, 3, 0)]
        [InlineData(3, 4, 3, 1)]
        public async Task Run(
            int numberOfProducts1Purchased,
            int numberOfProducts2Purchased,
            int requiredQuantityOfProduct1ToReceiveDiscount,
            int numberOfProducts2ThatShouldReceiveDiscount)
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var smartQueries = new SmartQueries(db);
            var discountService = new DiscountService(smartQueries);

            var product1 = new Product { Id = 1};
            var product2 = new Product { Id = 2};

            var basket = new Basket { Items = new List<BasketItem>() };

            for (var i = 0; i < numberOfProducts1Purchased; i++)
                basket.Items.Add(new BasketItem { ProductId = product1.Id });

            for (var i = 0; i < numberOfProducts2Purchased; i++)
                basket.Items.Add(new BasketItem { ProductId = product2.Id });

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
        }
    }
}
