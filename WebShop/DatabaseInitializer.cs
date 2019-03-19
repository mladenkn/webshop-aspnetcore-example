using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop
{
    public class DatabaseInitializer
    {
        public async Task Initialize(IUnitOfWork unitOfWork)
        {
            var butter = new Product {RegularPrice = 0.8m};
            var milk = new Product {RegularPrice = 1.15m };
            var bread = new Product {RegularPrice = 1m};

            unitOfWork.AddRange(new []{ butter, milk, bread });
            await unitOfWork.PersistChanges();

            var discounts = new[]
            {
                new Discount
                {
                    RequiredProductId = butter.Id,
                    RequiredProductRequiredQuantity = 2,
                    TargetProductId = bread.Id,
                    TargetProductQuantity = 1,
                    Value = 50
                },
                new Discount
                {
                    RequiredProductId = milk.Id,
                    RequiredProductRequiredQuantity = 3,
                    TargetProductId = milk.Id,
                    TargetProductQuantity = 4,
                    Value = 100
                },
            };

            unitOfWork.AddRange(discounts);
            await unitOfWork.PersistChanges();
        }
    }
}
