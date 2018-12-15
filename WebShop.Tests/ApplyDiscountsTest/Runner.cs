using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebShop.DataAccess;
using WebShop.Logic;
using WebShop.Models;

namespace WebShop.Tests.ApplyDiscountsTest
{
    internal struct Arguments
    {
        public int NumberOfBreadsPurchased { get; set; }
        public int NumberOfButterPurchased { get; set; }
        public int RequiredQuantityOfBread { get; set; }
        public int NumberOfButtersThatShouldReceiveDiscount { get; set; }
    }

    internal class Runner
    {
        internal static async Task Run(Arguments args)
        {
            var db = TestServiceFactory.Database();
            var smartQueries = new SmartQueries(db);
            var discountService = new DiscountService(smartQueries);

            var bread = new Product { Id = 1, Name = "bread" };
            var butter = new Product { Id = 2, Name = "butter" };

            var basket = new Basket { Items = new List<BasketItem>() };

            for (var i = 0; i < args.NumberOfBreadsPurchased; i++)
                basket.Items.Add(new BasketItem { ProductId = bread.Id });

            for (var i = 0; i < args.NumberOfButterPurchased; i++)
                basket.Items.Add(new BasketItem { ProductId = butter.Id });

            var discount = new Discount
            {
                Id = 1,
                RequiredProductId = bread.Id,
                TargetProductId = butter.Id,
                RequiredProductRequiredQuantity = args.RequiredQuantityOfBread,
                TargetProductQuantity = args.NumberOfButtersThatShouldReceiveDiscount
            };

            db.Add(basket);
            db.AddRange(bread, butter);
            db.AddRange(basket.Items);
            db.Add(discount);
            db.SaveChanges();

            var appliedDiscounts = await discountService.ApplyDiscounts(basket);

            appliedDiscounts.Should().HaveCount(args.NumberOfButtersThatShouldReceiveDiscount);
            appliedDiscounts.All(ap => ap.BasketItem.ProductId == butter.Id &&
                                       ap.DiscountId == discount.Id)
                .Should().BeTrue();
        }
    }
}
