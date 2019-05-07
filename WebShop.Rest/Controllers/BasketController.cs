using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebShop.Services;

namespace WebShop.Rest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BasketController : ControllerBase
    {
        private readonly IRequestExecutor _requestExecutor;
        private readonly ISafeRunner _safeRunner;
        private readonly IBasketService _basketService;

        public BasketController(IRequestExecutor requestExecutor, ISafeRunner safeRunner, IBasketService basketService)
        {
            _requestExecutor = requestExecutor;
            _safeRunner = safeRunner;
            _basketService = basketService;
        }

        [HttpPost]
        public Task AddItem(int productId) => _safeRunner.Run(() => _requestExecutor.AddItemToBasket(productId), Ok);

        [HttpDelete]
        public Task RemoveItem(int itemId) => _safeRunner.Run(() => _basketService.RemoveItem(itemId), Ok);

        [HttpGet]
        public Task<IActionResult> GetUsersBasketWithItemsAndDiscounts() =>
            _safeRunner.Run(() => _requestExecutor.GetUsersBasketWithItemsAndDiscounts(), Ok);
    }
}