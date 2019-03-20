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

            unitOfWork.AddRange(butter, milk, bread);
            await unitOfWork.PersistChanges();

            var discount1 = new Discount(1);
            discount1.AddRequiredProduct(butter.Id, 2);
            discount1.AddMicroDiscount(butter.Id, 1, 50);

            var discount2 = new Discount(2);
            discount2.AddRequiredProduct(milk.Id, 3);
            discount2.AddMicroDiscount(milk.Id, 3, 100);

            unitOfWork.AddRange(discount1, discount2);
            await unitOfWork.PersistChanges();
        }
    }
}
