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
                BasketDiscount.New()
                    .Require(productId: butter.Id, requiredQuantity: 2)
                    .DiscountFor(productId: bread.Id, quantity: 1, value: 50)
                    .Build(),

                BasketDiscount.New()
                    .Require(productId: milk.Id, requiredQuantity: 3)
                    .DiscountFor(productId: milk.Id, quantity: 1, value: 100)
                    .Build(),
            };

            discounts.ForEach(unitOfWork.Add);

            await unitOfWork.PersistChanges();
        }
    }
}
