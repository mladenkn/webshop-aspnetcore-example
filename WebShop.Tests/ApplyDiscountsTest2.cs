using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Infrastructure.DataAccess;
using WebShop.Logic;
using WebShop.Models;
using Xunit;

namespace WebShop.Tests
{
    public class ApplyDiscountsTest2
    {
        [Fact]
        public async Task Run()
        {
            var products = new[] {1, 2, 3, 4}.Select(id => new Product {Id = id});

            var basket = new Basket {Id = 1};

            var basketItems = new[]
            {
                new BasketItem {ProductId = 1, BasketId = basket.Id},
                new BasketItem {ProductId = 1, BasketId = basket.Id},
                
                new BasketItem {ProductId = 2, BasketId = basket.Id},
                new BasketItem {ProductId = 3, BasketId = basket.Id},
                new BasketItem {ProductId = 3, BasketId = basket.Id},
            };

            var discount = new Discount();
            discount.AddRequiredProduct(productId: 1, requiredQuantity: 2);
            discount.AddMicroDiscount(productId: 2, quantity: 1, value: 0.5m);

            var db = TestServiceFactory.InMemoryDatabase();
            var unitOfWork = new Infrastructure.DataAccess.UnitOfWork(db);

            unitOfWork.AddRange(products);
            unitOfWork.Add(basket);
            unitOfWork.AddRange(basketItems);
            unitOfWork.Add(discount);

            var service = new DiscountService(new SmartQueries(db));

            await service.ApplyDiscounts(basket);
        }
    }
}
