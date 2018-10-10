using System;
using System.Linq;
using System.Threading.Tasks;
using ApplicationKernel.Infrastructure;
using ApplicationKernel.Infrastructure.RestApi;
using Microsoft.AspNetCore.Mvc;
using Utilities;
using WebShop.Baskets;
using WebShop.UseCases;

namespace WebShop.Web.Controllers
{
    [ApiController]
    public class OneController : ApiController
    {
        [Route("/discounts")]
        public Task<IActionResult> Post(AddDiscount.Request request) => Handle(request);

        [Route("/basketitems")]
        public Task<IActionResult> Post(AddBasketItem.Request request) => Handle(request);

        [Route("baskets")]
        public async Task<IActionResult> Get(int basketId)
        {
            var response = await Handle(new GetBasket.Request{Id = basketId});
            if (response is OkObjectResult result)
            {
                var basket = (Basket) result.Value;

                basket.Items.Must().NotBeNull();
                basket.Items.First().Discounts.Must().NotBeNull();
                basket.Items.First().Product.Must().NotBeNull();

                return SerializerOf<Basket>()
                    .IgnoreProperty("Items.BasketItemDiscounts")
                    .Serialize()
                    .WrapIntoOkObjectResult();
            }
            return response;
        }
    }
}
