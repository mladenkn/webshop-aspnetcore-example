using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop
{
    public static class DatabaseInitializer
    {
        public static async Task Initialize(IUnitOfWork unitOfWork)
        {
            var butter = new Product {RegularPrice = 0.8m};
            var milk = new Product {RegularPrice = 1.15m };
            var bread = new Product {RegularPrice = 1m};

            unitOfWork.AddRange(butter, milk, bread);
            await unitOfWork.PersistChanges();

            var discounts = new[]
            {
                new BasketDiscount
                {
                    Id = 1,
                    RequiredProductId = butter.Id,
                    TargetProductId = bread.Id,
                    RequiredPerOneDiscounted = 2,
                    TargetProductDiscountedBy = 0.5m
                },

                new BasketDiscount
                {
                    Id = 2,
                    RequiredProductId = milk.Id,
                    TargetProductId = milk.Id,
                    RequiredPerOneDiscounted = 3,
                    TargetProductDiscountedBy = 1
                },
            };

            discounts.ForEach(unitOfWork.Add);

            await unitOfWork.PersistChanges();
        }
    }
}
