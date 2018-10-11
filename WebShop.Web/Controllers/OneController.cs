using System.Threading.Tasks;
using ApplicationKernel.Infrastructure.RestApi;
using Microsoft.AspNetCore.Mvc;
using WebShop.UseCases;

namespace WebShop.Web.Controllers
{
    [ApiController]
    public class OneController : ApiController
    {
        [HttpPost]
        [Route("/discounts")]
        public Task<IActionResult> Post(AddDiscount.Request request) => Handle(request);

        [HttpPost]
        [Route("/basketitems")]
        public Task<IActionResult> Post(AddBasketItem.Request request) => Handle(request);

        [HttpGet]
        [Route("baskets")]
        public Task<IActionResult> Get(int basketId) => Handle(new GetBasket.Request{Id = basketId});
    }
}
