using System.Threading.Tasks;
using Utilities;
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

            unitOfWork.AddRange(butter, milk, bread);
            await unitOfWork.PersistChanges();

            var discounts = new[]
            {
                Discount.Create(1, (require, discountFor) =>
                {
                    require(butter.Id, 2);
                    discountFor(bread.Id, 1, 50);
                }),
                Discount.Create(2, (require, discountFor) =>
                {
                    require(milk.Id, 3);
                    discountFor(milk.Id, 1, 100);
                })
            };

            discounts.ForEach(unitOfWork.Add);

            await unitOfWork.PersistChanges();
        }
    }
}
