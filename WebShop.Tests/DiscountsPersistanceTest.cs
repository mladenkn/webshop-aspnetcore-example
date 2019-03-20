using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Utilities;
using WebShop.Infrastructure.DataAccess;
using WebShop.DataAccess;
using WebShop.Models;
using Xunit;
using UnitOfWork = WebShop.Infrastructure.DataAccess.UnitOfWork;

namespace WebShop.Tests
{
    public class DiscountsPersistanceTest
    {
        private int nextUniqueInt = 1;
        private decimal nextUniqueDecimal = 1;

        private Discount GenerateDiscount()
        {
            var d = Discount.New()
                .Id(nextUniqueInt++)
                .Require(nextUniqueInt++, nextUniqueInt++)
                .Require(nextUniqueInt++, nextUniqueInt++)
                .DiscountFor(nextUniqueInt++, nextUniqueInt++, nextUniqueDecimal++)
                .DiscountFor(nextUniqueInt++, nextUniqueInt++, nextUniqueDecimal++)
                .Build();
            return d;
        }

        [Fact]
        public async Task Run()
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var customMapper = new CustomMapper(TestServiceFactory.AutoMapper());

            var discounts = General.GenerateSequence(GenerateDiscount, 10);
            var unitOfWork = new UnitOfWork(db, customMapper);
            unitOfWork.AddRange(discounts);
            await unitOfWork.PersistChanges();

            var lowLevelQueries = new LowLevelQueries(db, customMapper);
            var discountsWhenRead = await lowLevelQueries.QueryDiscounts(rp => rp, md => md);

            foreach (var discount in discountsWhenRead)
            {
                discounts.Should().ContainSingle(d => d.Id == discount.Id);
                var discount_ = discounts.Single(d => d.Id == discount.Id);
                discount_.Should().BeEquivalentTo(discount);
            }
        }
    }
}
