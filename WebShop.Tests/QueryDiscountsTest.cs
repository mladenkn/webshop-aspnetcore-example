using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using WebShop.Infrastructure.DataAccess;
using WebShop.Models;
using Xunit;

namespace WebShop.Tests
{
    public class QueryDiscountsTest
    {
        [Fact]
        public void Run()
        {
            var db = TestServiceFactory.InMemoryDatabase();
            var customMapper = new CustomMapper(TestServiceFactory.AutoMapper());

            var unitOfWork = new UnitOfWork(db, customMapper);

            var discounts = new[]
            {
                Discount.Create(1, (requireProduct, discountFor) =>
                {
                    requireProduct(1, 1);
                })
            };

            var lowLevelQueries = new LowLevelQueries(db, customMapper);
        }
    }
}
