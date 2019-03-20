using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Utilities;
using WebShop.DataAccess;
using WebShop.Models;

namespace WebShop.Logic
{
    public interface IDiscountService
    {
        Task ApplyDiscounts(Basket basket);
    }

    public class DiscountService : IDiscountService
    {
        private readonly ISmartQueries _smartQueries;

        public DiscountService(ISmartQueries smartQueries)
        {
            _smartQueries = smartQueries;
        }

        public async Task ApplyDiscounts(Basket basket)
        {
            basket.Items.Must().NotBeNull();

            var possibleDiscounts = await _smartQueries.GetPossibleDiscountsFor(basket);
        }
    }
}
