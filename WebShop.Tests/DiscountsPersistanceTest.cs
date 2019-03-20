using System;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using WebShop.Infrastructure.DataAccess;
using WebShop.DataAccess;
using WebShop.Models;
using Xunit;
using UnitOfWork = WebShop.Infrastructure.DataAccess.UnitOfWork;

namespace WebShop.Tests
{
    public class DiscountsPersistanceTest
    {
        private readonly Random _rand = new Random();

        private int RandomInt() => _rand.Next(0, 100);
        private decimal RandomDecimal() => RandomInt() + (decimal)_rand.NextDouble();

        private int idForNextDiscount = 1;

        private Discount GenerateDiscount()
        {
            return Discount.New()
                .Id(idForNextDiscount++)
                .Require(RandomInt(), RandomInt())
                .DiscountFor(RandomInt(), RandomInt(), RandomDecimal())
                .Build();
        }

        [Fact]
        public async Task Run()
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var customMapper = new CustomMapper(TestServiceFactory.AutoMapper());

            var discounts = Enumerable.Range(0, 1).Select(_ => GenerateDiscount());
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
