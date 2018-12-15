using System.Linq;
using System.Threading.Tasks;
using WebShop.Models;

namespace WebShop.Logic
{
    public interface IBasketService
    {
        Task AddItem(int basketId, int productId);
    }

    public class BasketService : IBasketService
    {
        private readonly IDiscountService _discountService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IQueries _queries;
        private readonly IPricesService _pricesService;

        public BasketService(
            IDiscountService discountService, 
            IUnitOfWork unitOfWork, 
            IQueries queries,
            IPricesService pricesService)
        {
            _discountService = discountService;
            _unitOfWork = unitOfWork;
            _queries = queries;
            _pricesService = pricesService;
        }

        public async Task AddItem(int basketId, int productId)
        {
            var basketItem = new BasketItem {BasketId = basketId, ProductId = productId};
            var basket = await _queries.GetBasketWithItems(basketId);
            basket.Items.Add(basketItem);

            basketItem.Discounts = await _discountService.ApplyDiscounts(basket);

            _pricesService.RefreshPriceOf(basket);
            _pricesService.RefreshPriceOf(basketItem);

            // todo: persist to db
        }
    }
}
