using System;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using WebShop.DataAccess;
using WebShop.Infrastructure.DataAccess;

namespace WebShop.Tests
{
    public static class TestServiceFactory
    {
        public static WebShopDbContext InMemoryDatabase()
        {
            var dbOptions = new DbContextOptionsBuilder().UseInMemoryDatabase(Guid.NewGuid().ToString()).Options;
            var db = new WebShopDbContext(dbOptions);
            return db;
        }

        //public static IUnitOfWork UnitOfWork()
        //{
        //    var db = InMemoryDatabase();
        //    return new Infrastructure.DataAccess.UnitOfWork(db);
        //}

        public static IMapper AutoMapper()
        {
            var config = new MapperConfiguration(c => { c.AddProfile<MapperProfile>(); });
            return config.CreateMapper();
        }
    }
}
