using System.Threading.Tasks;
using ApplicationKernel.Infrastructure.RestApi;
using Microsoft.AspNetCore.Mvc;
using WebShop.Features;

namespace WebShop.Infrastructure.RestApi.Controllers
{
    [ApiController]
    public class Controller
    {
        private readonly IApiRequestHandler _handler;

        public Controller(IApiRequestHandler handler)
        {
            _handler = handler;
        }

        [HttpPost]
        [Route("/discounts")]
        public Task<IActionResult> Post(AddDiscount.Request request) => _handler.Handle(request);

        [HttpPost]
        [Route("/basketitems")]
        public Task<IActionResult> Post(AddBasketItem.Request request) => _handler.Handle(request);

        [HttpGet]
        [Route("baskets")]
        public Task<IActionResult> Get(int basketId) => _handler.Handle(new GetBasket.Request{Id = basketId});
    }
}
